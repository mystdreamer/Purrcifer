using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_Math;

/// <summary>
/// Class defining polar functions.
/// </summary>
public class PolarFunctions2D
{
    public static readonly float GOLDEN_RATIO = 1.618F;

    /// <summary>
    /// https://mathworld.wolfram.com/ArchimedeanSpiral.html
    /// </summary>
    /// <param name="theta"> The angle of theta in radians. </param>
    /// <param name="amplitude">     The amplitude of the point.    </param>
    /// <param name="n">     The tightness of the spiral.   </param>
    /// <returns> PolarCoord2D defining a polar point on the spiral. </returns>
    public static float ArchimedeanSpiral(float theta, float amplitude, float n) => 
        amplitude * Mathf.Pow(theta, 1 / n);

    /// <summary>
    /// https://mathworld.wolfram.com/ArchimedesSpiral.html
    /// </summary>
    /// <param name="theta"> The angle of theta in radians. </param>
    /// <param name="amplitude"> The radius of the point. </param>
    /// <returns> PolarCoord2D defining a polar point on the spiral. </returns>
    public static float ArchimedesSpiral(float theta, float amplitude) => 
        amplitude * theta;

    /// <summary>
    /// https://mathworld.wolfram.com/Atom-Spiral.html
    /// </summary>
    /// <param name="theta"> The angle of theta in radians. </param>
    /// <param name="amplitude"> The radius of the point. </param>
    /// <returns> PolarCoord2D defining a polar point on the spiral. </returns>
    public static float AtomSpiral(float theta, float amplitude) => 
        theta / (theta - amplitude);

    /// <summary>
    /// https://mathworld.wolfram.com/Bifoliate.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float Bifoliate(float theta, float amplitude)
    {
        float sin = Mathf.Sin(theta);
        sin *= sin;
        float numerator = 8 * (Mathf.Cos(theta) * (sin * sin));
        float denumerator = 3 + Mathf.Cos(4 * theta);
        return (numerator / denumerator) * amplitude;
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Bifolium.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float Bifolium(float theta, float amplitude)
    {
        float sin = Mathf.Sin(theta);
        return 4 * amplitude * (sin * sin) * Mathf.Cos(theta);
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Butterfly_curve_(transcendental)
    /// https://mathworld.wolfram.com/ButterflyCurve.html
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float ButterflyCurve(float theta)
    {
        float doubleAngle = (2 * theta);
        float logResult = Mathf.Log(Mathf.Sin(theta));
        float cos = 2 * Mathf.Cos(4 * theta);
        float sin = Mathf.Sin((doubleAngle - Mathf.PI) / 24);
        float sinPow = sin * sin * sin * sin * sin;
        return logResult - cos + sinPow;
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Cardioid.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float CardioidCuspRight(float theta, float amplitude) => 
        2 * amplitude * (1 - Mathf.Cos(theta));

    /// <summary>
    /// https://mathworld.wolfram.com/Cardioid.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float CardioidCuspUp(float theta, float amplitude) => 
        2 * amplitude * (1 - Mathf.Sin(theta));

    /// <summary>
    /// https://mathworld.wolfram.com/Cardioid.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float CardioidCuspDown(float theta, float amplitude) =>
        2 * amplitude * (1 + Mathf.Sin(theta));

    /// <summary>
    /// https://mathworld.wolfram.com/Cardioid.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float CardioidCuspLeft(float theta, float amplitude) =>
        2 * amplitude * (1 + Mathf.Cos(theta));

    /// <summary>
    /// https://mathworld.wolfram.com/CissoidofDiocles.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float CissoidOfDiocles(float theta, float amplitude)
    {
        float sin = Mathf.Sin(theta);
        return 2 * amplitude * sin * Mathf.Tan(theta);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/CayleysSextic.html
    /// https://en.wikipedia.org/wiki/Cayley%27s_sextic#:~:text=Equations%20of%20the%20curve&text=4(x2%20%2B%20y2,2%20%2B%20y2)2%20.&text=y%20%3D%20cos3t%20sin%203t.
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float CayleysSextic(float theta, float amplitude)
    {
        float cos = Mathf.Cos(theta / 3);

        return 4 * amplitude * (cos * cos * cos);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Cochleoid.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitide"></param>
    /// <returns></returns>
    public static float Cochleoid(float theta, float amplitide) =>
        (amplitide * Mathf.Sin(theta)) / theta;

    /// <summary>
    /// https://mathworld.wolfram.com/CycloidofCeva.html
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float CycloidOfCeva(float theta) =>
        1 + (2 * Mathf.Cos(2 * theta));

    /// <summary>
    /// https://mathworld.wolfram.com/ConchoidofdeSluze.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float ConchoidOfDeSluze(float theta, float amplitude) =>
        C_MathF.Sec(theta) + (amplitude * C_MathF.Cos(theta));

    /// <summary>
    /// https://mathworld.wolfram.com/DevilsCurve.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float DevilsCurve(float theta, float amplitude, float b)
    {
        float aSqr = (amplitude * amplitude);
        float bSqr = (b * b);
        float cosAngle = C_MathF.Cos(theta);
        float sinAngle = C_MathF.Sin(theta);
        float cosAngleSqr = cosAngle * cosAngle;
        float sinAngleSqr = sinAngle * sinAngle;
        float numerator = (aSqr * sinAngleSqr) - (bSqr * cosAngleSqr);
        float denumerator = sinAngleSqr - cosAngleSqr;
        return Mathf.Sqrt(numerator / denumerator);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/DuererFolium.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float DurerFolium(float theta, float amplitude) =>
        amplitude * C_MathF.Sin(theta / 2);

    /// <summary>
    /// https://mathworld.wolfram.com/EightCurve.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="pos"></param>
    /// <param name="neg"></param>
    public static PolarMultiOutput EightCurve(float theta, float amplitude)
    {
        float aSqr = amplitude * amplitude;
        float sec = C_MathF.Sec(theta);
        float secantCubed = sec * sec * sec * sec;
        float pos = Mathf.Sqrt(aSqr * secantCubed * Mathf.Cos(2 * theta));
        return new PolarMultiOutput(pos, -pos);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Epispiral.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static float Epispiral(float theta, float amplitude, int n) => 
        amplitude * C_MathF.Sec(n * theta);

    /// <summary>
    /// https://mathworld.wolfram.com/FermatsSpiral.html
    /// </summary>
    /// <param name="a"></param>
    /// <param name="theta"></param>
    /// <param name="pos"></param>
    /// <param name="neg"></param>
    public static PolarMultiOutput FermatsSpiral(float a, float theta)
    {
        float val = (a * a) * theta;
        return new PolarMultiOutput(Mathf.Sqrt(val), -Mathf.Sqrt(val));
    }

    /// <summary>
    /// https://mathworld.wolfram.com/FoliumofDescartes.html
    /// </summary>
    /// <param name="a"></param>
    /// <param name="theta"></param>
    public static PolarMultiOutput FoliumofDescartes(float a, float theta)
    {
        float tanTheta = Mathf.Tan(theta);
        float tanThetaCubed = tanTheta * tanTheta * tanTheta;
        float pos = (3 * a * C_MathF.Sec(theta) * tanTheta) / (1 + tanThetaCubed);
        return new PolarMultiOutput(pos, -pos);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/FreethsNephroid.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float FreethsNeproid(float theta, float amplitude)
    {
        float sin = 2 * C_MathF.Sin(0.5F * theta);
        return amplitude * (1 + sin);
    }

    /// <summary>
    /// The Garfield Curve as defined by Wolfram. 
    /// https://mathworld.wolfram.com/GarfieldCurve.html
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float GarfieldCurves(float theta) =>
        theta * C_MathF.Cos(theta);

    /// <summary>
    /// https://en.wikipedia.org/wiki/Golden_spiral
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"> Inital radius of the spiral. </param>
    /// <param name="b"> Right angle from theta. </param>
    /// <returns></returns>
    public static float GoldenSpiral(float theta, float amplitude, float b) =>
        amplitude * Mathf.Log(b * theta);

    /// <summary>
    /// Hippopede Curve: https://mathworld.wolfram.com/Hippopede.html
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="theta"></param>
    /// <param name="pos"></param>
    /// <param name="neg"></param>
    public static PolarMultiOutput HippopedeCurve(float a, float b, float theta)
    {
        float sin = Mathf.Sin(theta);
        float sinSquared = sin * sin;
        float rSquare = 4 * b * (a - (b * sinSquared));

        return new PolarMultiOutput(Mathf.Sqrt(rSquare), -Mathf.Sqrt(rSquare));
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Hyperbola.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float Hyperbola(float theta, float amplitude) =>
        (amplitude * (Mathf.Log(2) - 1) / 1 - Mathf.Log(1) * Mathf.Cos(theta));

    /// <summary>
    /// https://mathworld.wolfram.com/HyperbolicSpiral.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float HyperbolicSpiral(float theta, float amplitude) => 
        amplitude / theta;

    /// <summary>
    /// https://mathworld.wolfram.com/KampyleofEudoxus.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float KaympleOfEudoxus(float theta, float amplitude) => 
        amplitude * (C_MathF.Sec(theta) * C_MathF.Sec(theta));

    /// <summary>
    /// https://mathworld.wolfram.com/KappaCurve.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float KappaCurveFunc(float theta, float amplitude) =>
        amplitude * C_MathF.Cot(theta);

    /// <summary>
    /// https://mathworld.wolfram.com/KeplersFolium.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float KeplersFoliumFunc(float theta, float amplitude, float b)
    {
        float sinSquared = C_MathF.Sin(theta - b) * C_MathF.Sin(theta - b);
        float innerProd = 4 * amplitude * sinSquared;
        return C_MathF.Cos(innerProd);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Limacon.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Limacon(float theta, float amplitude, float b) =>
        b + (amplitude * C_MathF.Cos(theta));

    /// <summary>
    /// https://mathworld.wolfram.com/LimaconTrisectrix.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float LimaconTrisectrix(float theta, float amplitude) =>
        amplitude * (1 + (2 * C_MathF.Cos(theta)));

    /// <summary>
    /// https://mathworld.wolfram.com/LogarithmicSpiral.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float LogaritmicSpiral(float theta, float amplitude, float b) =>
        amplitude * Mathf.Log(b * theta);

    /// <summary>
    /// https://mathworld.wolfram.com/Lemniscate.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    public static PolarMultiOutput Lemniscate(float theta, float amplitude)
    {
        //float inner = Mathf.Sqrt(Mathf.Cos(2 * theta));
        float inner = Mathf.Sqrt(Mathf.Sin(2 * theta));
        return new PolarMultiOutput(amplitude * inner, amplitude * -inner);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/MaclaurinTrisectrix.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float MaclaurinTrisectrix(float theta, float amplitude)
    {
        float innerProduct = 0.33333333333F * theta;

        return -amplitude * C_MathF.Sec(innerProduct);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/MalteseCrossCurve.html
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float MalteseCross(float theta) => 
        2 / Mathf.Sqrt(Mathf.Sin(4 * theta));

    /// <summary>
    /// https://mathworld.wolfram.com/Neoid.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Neoid(float theta, float amplitude, float b) => 
        (amplitude* theta) + b;

    /// <summary>
    /// https://mathworld.wolfram.com/Ophiuride.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Ophiuride(float theta, float amplitude, float b)
    {
        float innerProd = (b * Mathf.Sin(theta) - (amplitude * Mathf.Cos(theta)));
        return innerProd * Mathf.Tan(theta);
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Quadrifolium.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float Quadrifolium(float theta, float amplitude) => 
        amplitude * Mathf.Cos(2 * theta);

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="theta"></param>
    ///// <param name="amplitude"></param>
    ///// <param name="k"></param>
    ///// <returns></returns>
    //public static float Rose(float theta, float amplitude, float k) => 
    //    amplitude * Mathf.Sin(k * theta);

    /// <summary>
    /// https://mathworld.wolfram.com/SwastikaCurve.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="pos"></param>
    /// <param name="neg"></param>
    public static PolarMultiOutput SwastikaCurve(float theta)
    {
        float cos = Mathf.Cos(theta);
        float sin = Mathf.Sin(theta);
        float sinCubed = (sin * sin * sin * sin);
        float cosCubed = (cos * cos * cos * cos);
        float rSquare = (sin * cos) / (sinCubed - cosCubed);
        return new PolarMultiOutput(Mathf.Sqrt(rSquare), -Mathf.Sqrt(rSquare));
    }

    /// <summary>
    /// https://mathworld.wolfram.com/ScarabaeusCurve.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float ScarabaeusCurve(float theta, float amplitude, float b) => 
        b * Mathf.Cos(2 * theta) - amplitude * Mathf.Cos(theta);

    /// <summary>
    /// https://mathworld.wolfram.com/SemicubicalParabola.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float SemicubicalParabola(float theta, float amplitude)
    {
        float tan = Mathf.Tan(theta);
        float tanSqr = tan * tan;
        return (tanSqr * C_MathF.Sec(theta)) / amplitude;
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Strophoid.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Strophoid(float theta, float amplitude, float b)
    {
        float numerator = b * C_MathF.Sin(amplitude - (2 * theta));
        float d = C_MathF.Sin(amplitude - theta);
        return numerator / d;
    }

    /// <summary>
    /// https://mathworld.wolfram.com/Trifolium.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float Trifolium(float theta, float amplitude) => 
        -amplitude * Mathf.Cos(3 * theta);

    /// <summary>
    /// https://mathworld.wolfram.com/TschirnhausenCubic.html
    /// </summary>
    /// <param name="theta"></param>
    /// <param name="amplitude"></param>
    /// <returns></returns>
    public static float TschirnhausenCubic(float theta, float amplitude)
    {
        float secant = 1 / Mathf.Cos(theta / 3);
        float secantCubed = secant * secant * secant;

        return amplitude * secantCubed;
    }

    /// <summary>
    /// https://mathworld.wolfram.com/WattsCurve.html
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    public static PolarMultiOutput WattsCurve(float angle, float a, float b, float c)
    {
        float a2 = a * a;
        float b2 = b * b;
        float c2 = c * c;
        float cos2 = Mathf.Cos(angle) * Mathf.Cos(angle);
        float IP = a * Mathf.Pow(Mathf.Sin(angle) + Mathf.Sqrt(c2 - a2 * cos2), 2);
        float IN = a * Mathf.Pow(Mathf.Sin(angle) - Mathf.Sqrt(c2 - a2 * cos2), 2);
        return new PolarMultiOutput(
            Mathf.Sqrt(b2 - IP),
            -Mathf.Sqrt(b2 - IP),
            Mathf.Sqrt(b2 - IN),
            -Mathf.Sqrt(b2 - IN));
    }
}
