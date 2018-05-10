using UnityEngine;

//
// Summary:
//     ///
//     Set of easing functions to ease values in a predetermined amount of time.
//     ///
public static class Easing
{
    // ** Basic easing functions ** //
    public static float InCubic(float t, float b, float c, float d)
    {
        c = c - b;
        var tc = (t /= d) * t * t;
        return b + c * (tc);
    }
    public static Vector2 InCubic(float t, Vector2 b, Vector2 c, float d)
    {
        c = c - b;
        var tc = (t /= d) * t * t;
        return b + c * (tc);
    }
    public static Vector3 InCubic(float t, Vector3 b, Vector3 c, float d)
    {
        c = c - b;
        var tc = (t /= d) * t * t;
        return b + c * (tc);
    }
    public static Color InCubic(float t, Color b, Color c, float d)
    {
        c = c - b;
        var tc = (t /= d) * t * t;
        return b + c * (tc);
    }

    public static float OutCubic(float t, float b, float c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc + -3 * ts + 3 * t);
    }
    public static Vector2 OutCubic(float t, Vector2 b, Vector2 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc + -3 * ts + 3 * t);
    }
    public static Vector3 OutCubic(float t, Vector3 b, Vector3 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc + -3 * ts + 3 * t);
    }
    public static Color OutCubic(float t, Color b, Color c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc + -3 * ts + 3 * t);
    }

    public static float InQuartic(float t, float b, float c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        return b + c * (ts * ts);
    }
    public static Vector2 InQuartic(float t, Vector2 b, Vector2 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        return b + c * (ts * ts);
    }
    public static Vector3 InQuartic(float t, Vector3 b, Vector3 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        return b + c * (ts * ts);
    }
    public static Color InQuartic(float t, Color b, Color c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        return b + c * (ts * ts);
    }

    public static float OutQuartic(float t, float b, float c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (-1 * ts * ts + 4 * tc + -6 * ts + 4 * t);
    }
    public static Vector2 OutQuartic(float t, Vector2 b, Vector2 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (-1 * ts * ts + 4 * tc + -6 * ts + 4 * t);
    }
    public static Vector3 OutQuartic(float t, Vector3 b, Vector3 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (-1 * ts * ts + 4 * tc + -6 * ts + 4 * t);
    }
    public static Color OutQuartic(float t, Color b, Color c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (-1 * ts * ts + 4 * tc + -6 * ts + 4 * t);
    }

    public static float InQuintic(float t, float b, float c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc * ts);
    }
    public static Vector2 InQuintic(float t, Vector2 b, Vector2 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc * ts);
    }
    public static Vector3 InQuintic(float t, Vector3 b, Vector3 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc * ts);
    }
    public static Color InQuintic(float t, Color b, Color c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc * ts);
    }

    public static float OutQuintic(float t, float b, float c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
    }
    public static Vector2 OutQuintic(float t, Vector2 b, Vector2 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
    }
    public static Vector3 OutQuintic(float t, Vector3 b, Vector3 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
    }
    public static Color OutQuintic(float t, Color b, Color c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
    }

    // ** Special easing functions ** //
    public static float FallingPlatformDown(float t, float b, float c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (7.295f * tc * ts + -23.6825f * ts * ts + 30.18f * tc + -19.19f * ts + 6.3975f * t);
    }
    public static Vector3 FallingPlatformDown(float t, Vector3 b, Vector3 c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (7.295f * tc * ts + -23.6825f * ts * ts + 30.18f * tc + -19.19f * ts + 6.3975f * t);
    }
    public static Color FallingPlatformDown(float t, Color b, Color c, float d)
    {
        c = c - b;
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (7.295f * tc * ts + -23.6825f * ts * ts + 30.18f * tc + -19.19f * ts + 6.3975f * t);
    }
}
