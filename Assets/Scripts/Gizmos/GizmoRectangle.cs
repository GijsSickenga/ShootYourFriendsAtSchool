using UnityEngine;

public static class GizmoRectangle
{
    //
    // Summary:
    //     ///
    //     Draws a rectangle with four Gizmo.DrawLine calls.
    //     Should only be called from OnDrawGizmos() and OnDrawGizmosSelected().
    //     ///
    //
    // Parameters:
    //   position:
    //     The position to draw the rectangle at.
    //     This represents the top left corner of the rectangle.
    //
    //   width:
    //     The width of the rectangle in Unity scene units.
    //
    //   height:
    //     The height of the rectangle in Unity scene units.
    //
    public static void Draw(Vector3 position, float width, float height)
    {
        Vector3[] vertices =
        {
            // Left line
            position,
            new Vector3(position.x, position.y - height, position.z),

            // Top line
            position,
            new Vector3(position.x + width, position.y, position.z),

            // Bottom line
            new Vector3(position.x, position.y - height, position.z),
            new Vector3(position.x + width, position.y - height, position.z),
            
            // Right line
            new Vector3(position.x + width, position.y, position.z),
            new Vector3(position.x + width, position.y - height, position.z)
        };

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[++i]);
        }
    }
    //
    // Summary:
    //     ///
    //     Draws a rectangle with four Gizmo.DrawLine calls.
    //     Should only be called from OnDrawGizmos() and OnDrawGizmosSelected().
    //     Z position is set to 0 for Vector3 calls.
    //     ///
    //
    // Parameters:
    //   position:
    //     The position to draw the rectangle at.
    //     This represents the top left corner of the rectangle.
    //
    //   width:
    //     The width of the rectangle in Unity scene units.
    //
    //   height:
    //     The height of the rectangle in Unity scene units.
    //
    public static void Draw(Vector2 position, float width, float height)
    {
        Vector3[] vertices =
        {
            // Left line
            position,
            new Vector3(position.x, position.y - height, 0),

            // Top line
            position,
            new Vector3(position.x + width, position.y, 0),

            // Bottom line
            new Vector3(position.x, position.y - height, 0),
            new Vector3(position.x + width, position.y - height, 0),
            
            // Right line
            new Vector3(position.x + width, position.y, 0),
            new Vector3(position.x + width, position.y - height, 0)
        };

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[++i]);
        }
    }
    //
    // Summary:
    //     ///
    //     Draws a rectangle with four Gizmo.DrawLine calls.
    //     Should only be called from OnDrawGizmos() and OnDrawGizmosSelected().
    //     Z position is set to 0 for Vector3 calls.
    //     ///
    //
    // Parameters:
    //   x:
    //     The x position of the top left corner of the rectangle.
    //
    //   y:
    //     The y position of the top left corner of the rectangle.
    //
    //   width:
    //     The width of the rectangle in Unity scene units.
    //
    //   height:
    //     The height of the rectangle in Unity scene units.
    //
    public static void Draw(float x, float y, float width, float height)
    {
        Vector3[] vertices =
        {
            // Left line
            new Vector3(x, y, 0),
            new Vector3(x, y - height, 0),

            // Top line
            new Vector3(x, y, 0),
            new Vector3(x + width, y, 0),

            // Bottom line
            new Vector3(x, y - height, 0),
            new Vector3(x + width, y - height, 0),
            
            // Right line
            new Vector3(x + width, y, 0),
            new Vector3(x + width, y - height, 0)
        };

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[++i]);
        }
    }

    //
    // Summary:
    //     ///
    //     Draws a rectangle with Draw() and subsequently draws two diagonal lines across the rectangle.
    //     Should only be called from OnDrawGizmos() and OnDrawGizmosSelected().
    //     ///
    //
    // Parameters:
    //   position:
    //     The position to draw the rectangle at.
    //     This represents the top left corner of the rectangle.
    //
    //   width:
    //     The width of the rectangle in Unity scene units.
    //
    //   height:
    //     The height of the rectangle in Unity scene units.
    //
    public static void DrawCrossed(Vector3 position, float width, float height)
    {
        // Draw the rectangle
        Draw(position, width, height);

        // Top left to bottom right vertex
        Gizmos.DrawLine(position,
                        new Vector3(position.x + width, position.y - height, position.z));

        // Bottom left to top right vertex
        Gizmos.DrawLine(new Vector3(position.x, position.y - height, position.z),
                        new Vector3(position.x + width, position.y, position.z));
    }

    //
    // Summary:
    //     ///
    //     Draws a rectangle with Draw() and subsequently draws a hatching pattern across the rectangle.
    //     Should only be called from OnDrawGizmos() and OnDrawGizmosSelected().
    //     ///
    //
    // Parameters:
    //   position:
    //     The position to draw the rectangle at.
    //     This represents the top left corner of the rectangle.
    //
    //   width:
    //     The width of the rectangle in Unity scene units.
    //
    //   height:
    //     The height of the rectangle in Unity scene units.
    //
    //   hatchDensity:
    //     The density of the hatch.
    //
    public static void DrawHatched(Vector3 position, float width, float height, int hatchDensity = 4)
    {
        // Draw the rectangle
        Draw(position, width, height);

        // Draw the hatching
        for (int i = 0; i <= hatchDensity; i++)
        {
            // Left line to top line
            Gizmos.DrawLine
            (
                new Vector3(position.x, position.y - (height / hatchDensity * i), position.z),
                new Vector3(position.x + (width / hatchDensity * i), position.y, position.z)
            );

            // Left line to bottom line
            Gizmos.DrawLine
            (
                new Vector3(position.x, position.y - (height / hatchDensity * i), position.z),
                new Vector3(position.x + width - (width / hatchDensity * i), position.y - height, position.z)
            );

            // Top line to right line
            Gizmos.DrawLine
            (
                new Vector3(position.x + (width / hatchDensity * i), position.y, position.z),
                new Vector3(position.x + width, position.y - height + (height / hatchDensity * i), position.z)
            );

            // Bottom line to right line
            Gizmos.DrawLine
            (
                new Vector3(position.x + (width / hatchDensity * i), position.y - height, position.z),
                new Vector3(position.x + width, position.y - (height / hatchDensity * i), position.z)
            );
        }
    }
}
