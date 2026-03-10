using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace _3dgame {
    public class Player : GameObject {
        public float ForwardDirection { get; set; }
        private bool jumpStarted = false;
        private float movementDirection = 0;
        private bool reachedApex = false;
        private bool reachedBottom = true;
        private Vector3 startPosition = new Vector3(30, GameConstants.HeightOffset, 125);
        public Player() : base() {
            ForwardDirection = 0.0f;
            Position = startPosition;
        }

        public void Update(KeyboardState keyboardState, float cameraYaw, BoundingSphere boundingSphere) {
            Vector3 futurePosition = Position;
            Vector3 movement = Vector3.Zero;

            
            BoundingSphere updatedSphere;
            updatedSphere = BoundingSphere;
            updatedSphere.Center.X = Position.X;
            updatedSphere.Center.Y = Position.Y;
            updatedSphere.Radius = 1;
            BoundingSphere = new BoundingSphere(updatedSphere.Center, updatedSphere.Radius);

            if (keyboardState.IsKeyDown(Keys.A)) {
                movement.X = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.D)) {
                movement.X = 1;
            }
            if (keyboardState.IsKeyDown(Keys.W)) {
                movement.Z = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.S)) {
                movement.Z = 1;
            }
            if (keyboardState.IsKeyDown(Keys.Space)) {
                jumpStarted = true;
            }

            if (jumpStarted) {
                movement.Y = PlayerJump();
            }

            ForwardDirection = cameraYaw;
            Matrix orientationMatrix = Matrix.CreateRotationY(ForwardDirection);

            Vector3 speed = Vector3.Transform(movement, orientationMatrix);
            speed *= GameConstants.Velocity;
            futurePosition = Position + speed;

            if (ValidateMovement(futurePosition, boundingSphere)) {
                Position = futurePosition;
            }
        }

        private bool ValidateMovement(Vector3 futurePosition, BoundingSphere boundingSphere) {
            // do not allow off-terrain movement
            if ((Math.Abs(futurePosition.X) > GameConstants.MaxRange) || (Math.Abs(futurePosition.Z) > GameConstants.MaxRange)) {
                return false;
            }
            // house collision
            if (boundingSphere.Intersects(this.BoundingSphere)) {
                return false;
            }

            return true;
        }

        private float PlayerJump() {
            Vector3 position = Position;
            if (reachedApex == false && reachedBottom == true) {
                movementDirection = 0.8f;
            }
            if (position.Y >= GameConstants.JumpHeight && reachedApex == false) {
                reachedApex = true;
                reachedBottom = false;
            }
            if (reachedApex == true) {
                movementDirection = -0.9f;
            }
            if (position.Y <= GameConstants.HeightOffset && reachedBottom == false) {
                jumpStarted = false;
                reachedBottom = true;
                reachedApex = false;
                movementDirection = 0;
            }
            return movementDirection;
        }
    }
}