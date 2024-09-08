using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Purrcifer.Data.Xml;

[System.Serializable]
public class PurrciferUI_EventData
{
    public bool validated = false;
    public bool validName = true;
    public bool validID = true;
    public string name;
    public int id;
    public bool state;

    public string GetLabel(bool selected)
    {
        string prefix = (selected) ? "<color=yellow> -- </color>" : "";
        string suffix = (validated) ? "<color=yellow> [VALID] </color>" : "<color=red> [INVALID] </color>";
        return prefix + name + " [id: " + id + "]" + suffix;
    }

    public TextField GetTextField()
    {
        return new TextField("Event Name") { value = name };
    }

    public IntegerField GetIntField()
    {
        return new IntegerField("Event ID") { value = id };
    }

    public void Validate(PurrciferUI_EventData eventData)
    {
        validName = (name != eventData.name);
        validID = (id != eventData.id);
        validated = (validName && validID);
    }
}

[System.Serializable]
public class PurrciferUI_EventList
{
    [SerializeField] private List<PurrciferUI_EventData> eventData = new List<PurrciferUI_EventData>();

    public void Add()
    {
        eventData.Add(new PurrciferUI_EventData() { name = "New Event Data.", id = 1001, state = false });
    }

    public void Remove(int id)
    {
        if (EventExists(id))
        {
            List<PurrciferUI_EventData> eTemp = new List<PurrciferUI_EventData>();

            for (int i = 0; i < eventData.Count; i++)
            {
                if (i != id)
                    eTemp.Add(eventData[i]);
            }

            eventData = eTemp;
        }
    }

    public bool EventExists(int id)
    {
        if (id >= 0 && id <= eventData.Count)
            if (eventData[id] != null)
                return true;
        return false;
    }

    private bool Validate()
    {
        PurrciferUI_EventData data;
        for (int i = 0; i < eventData.Count; i++)
        {
            data = eventData[i];
            for (int j = 0; j < eventData.Count; j++)
            {
                if (j != i)
                {
                    data.Validate(eventData[j]);

                    if (!eventData[i].validated)
                        return false;
                }
            }
        }

        return true;
    }

    public GameEventDataXML GetEventXML()
    {
        GameEventDataXML edXML = new GameEventDataXML();
        List<GameEventXML> list = new List<GameEventXML>();

        for (int i = 0; i < eventData.Count; i++)
        {
            list.Add(new GameEventXML() { name = eventData[i].name, id = eventData[i].id, state = false });
        }
        edXML.events = list.ToArray();
        return edXML;
    }
}

public class PurrciferEventEditor : EditorWindow
{
    [SerializeField] private int m_selectedIndex = -1;
    [SerializeField] private bool isValidated = false;
    [SerializeField] private List<PurrciferUI_EventData> eventData = new List<PurrciferUI_EventData>();
    private VisualElement m_ItemSplitView;
    private VisualElement m_leftPane;
    private VisualElement m_rightPane;
    private TextField m_TextField;
    private IntegerField m_IntField;

    [MenuItem("Purrcifer/Event Editor")]
    public static void ShowEditorWindow()
    {
        EditorWindow wnd = GetWindow<PurrciferEventEditor>();
        wnd.titleContent = new GUIContent("Purrcifer - Event Editor");

        //Limit size of window. 
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }

    public void CreateGUI()
    {
        if (rootVisualElement != null)
        {
            rootVisualElement.Clear();
        }

        //Create menu and item layout split. 
        var splitView_Toolbar_Display = new TwoPaneSplitView(0, 25, TwoPaneSplitViewOrientation.Vertical);
        var toolbar = CreateToolbar();
        splitView_Toolbar_Display.Add(toolbar);

        //Generate Item Layout panels. 
        var splitViewItems = CreateItemView(m_selectedIndex);
        m_ItemSplitView = splitViewItems;
        splitView_Toolbar_Display.Add(splitViewItems);

        ((ListView)m_leftPane).makeItem = () => new Label();
        ((ListView)m_leftPane).bindItem = (item, index) => (item as Label).text = eventData[index].GetLabel((index == m_selectedIndex));
        ((ListView)m_leftPane).itemsSource = eventData;


        ((ListView)m_leftPane).selectionChanged += OnSelectionIndexChange;
        //((ListView)m_leftPane).selectedIndex = m_selectedIndex;
        rootVisualElement.Add(splitView_Toolbar_Display);
    }

    private Toolbar CreateToolbar()
    {
        var toolbar = new Toolbar();
        var addButton = new Button(() => { OnAdd(); }) { text = "Add Event" };
        var removeButton = new Button(() => { OnRemove(); }) { text = "Remove Event" };
        var verifyButton = new Button(() => { OnVerify(); }) { text = "Verify Events" };
        var saveButton = new Button(() => { OnSave(); }) { text = "Save Events" };
        toolbar.Add(addButton);
        toolbar.Add(removeButton);
        toolbar.Add(verifyButton);
        toolbar.Add(saveButton);
        return toolbar;
    }

    private TwoPaneSplitView CreateItemView(int selected)
    {
        var splitViewItems = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        m_leftPane = new ListView();
        m_rightPane = new ScrollView();
        splitViewItems.Add(m_leftPane);
        splitViewItems.Add(m_rightPane);

        if (selected >= 0 && selected < eventData.Count)
        {
            if (eventData[selected] != null)
            {
                var selectedEvent = eventData[selected];
                m_TextField = new TextField("Item Name");
                m_TextField.value = selectedEvent.name;
                m_IntField = new IntegerField("Item ID");
                m_IntField.value = selectedEvent.id;
                m_rightPane.Add(m_TextField);
                m_rightPane.Add(m_IntField);

                if (!selectedEvent.validName)
                {
                    m_rightPane.Add(new Label("[ERROR] - Event name is contained in the list. "));
                }

                if (!selectedEvent.validID)
                {
                    m_rightPane.Add(new Label("[ERROR] - Event ID is contained in the list. "));
                }

                m_TextField.RegisterCallback<ChangeEvent<string>>((e) => { ApplyData(); });
                m_IntField.RegisterCallback<ChangeEvent<int>>((e) => { ApplyData(); });
            }
        }

        return splitViewItems;
    }

    private void OnSelectionIndexChange(IEnumerable<object> selectedItems)
    {
        ApplyData();

        //Set new index and redraw. 
        m_selectedIndex = ((ListView)m_leftPane).selectedIndex;
        ((ListView)m_leftPane).selectionChanged += (items) => { m_selectedIndex = ((ListView)m_leftPane).selectedIndex; };
        CreateGUI();
    }

    private void OnAdd()
    {
        Debug.Log("Add Item Button clicked");
        Add();
        ApplyData();
        CreateGUI();
    }

    private void OnRemove()
    {
        if (m_selectedIndex < 0)
            return;

        Debug.Log("Remove Item Button clicked");
        ApplyData();

        Remove(m_selectedIndex);
        m_selectedIndex = -1;
        CreateGUI();

    }

    private void OnVerify()
    {
        Debug.Log("Verify List Button clicked");
        ApplyData();
        PurrciferUI_EventData data; 


        for (int i = 0; i < eventData.Count; i++)
        {
            data = eventData[i];
            for (int j = 0; j < eventData.Count; j++)
            {
                if(j != i)
                {
                    data.Validate(eventData[j]);

                    if (!eventData[i].validated)
                        break;
                }
            }
        }
        CreateGUI();
    }

    private void OnSave()
    {
        ApplyData();
        Debug.Log("Export Items Button clicked");

        for (int i = 0;i < eventData.Count; i++)
        {
            if (!eventData[i].validated)
            {
                Debug.LogError("Data not saved due to validation errors, fix errors and save again.");
                return;
            }
        }

        
    }

    private void ApplyData()
    {
        //Set old data. 
        if (m_selectedIndex >= 0)
        {
            eventData[m_selectedIndex].name = m_TextField.value;
            eventData[m_selectedIndex].id = m_IntField.value;
        }

    }

    private void Add()
    {
        eventData.Add(new PurrciferUI_EventData() { name = "New Event Data.", id = 1001, state = false });
    }

    private void Remove(int id)
    {
        if (EventExists(id))
        {
            List<PurrciferUI_EventData> eTemp = new List<PurrciferUI_EventData>();

            for (int i = 0; i < eventData.Count; i++)
            {
                if (i != id)
                    eTemp.Add(eventData[i]);
            }

            eventData = eTemp;
        }
    }

    private bool EventExists(int id)
    {
        if (id >= 0 && id <= eventData.Count)
            if (eventData[id] != null)
                return true;
        return false;
    }
}
