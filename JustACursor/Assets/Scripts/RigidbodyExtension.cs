using UnityEngine;

public static class RigidbodyExtension {
    
    public static void SetVelocity(this Rigidbody2D rb2d, Axis axis, float value) {
        Vector2 oldVelocity = rb2d.velocity;
        Vector2 newVelocity =
            new Vector2(
                axis == Axis.X ? value : oldVelocity.x,
                axis == Axis.Y ? value : oldVelocity.y);

        rb2d.velocity = newVelocity;
    }
}
