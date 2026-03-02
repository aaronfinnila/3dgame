using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3dgame {
    public class Dragon : GameObject {
        public float ForwardDirection { get; set; }
        private int updateCount = 0;
        private Vector3 startPosition = new Vector3(0, 0, 0);
        public Dragon() : base() {
            Position = startPosition;
            ForwardDirection = 0.0f;
        }

        public void LoadContent(ContentManager content, string modelName) {
            Model = content.Load<Model>(modelName);
            Position = Vector3.Down;
        }

        public void Draw(Matrix view, Matrix projection) {
            Matrix translateMatrix = Matrix.CreateTranslation(Position);
            Matrix worldMatrix = translateMatrix;
            Matrix correction = Matrix.CreateRotationX(MathHelper.ToRadians(-90));

            foreach (ModelMesh mesh in Model.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.World = correction * worldMatrix;
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                mesh.Draw();
            }
        }
        
        public void Update(KeyboardState keyboardState) {
            Vector3 futurePosition = Position;
            Vector3 movement = Vector3.Zero;
            updateCount++;
            if (keyboardState.IsKeyDown(Keys.Right)) {
                movement.X = -1;
            }
            else if(keyboardState.IsKeyDown(Keys.Left)) {
                movement.X = 1;
            }
            if (keyboardState.IsKeyDown(Keys.Down)) {
                movement.Z = -1;
            }
            else if(keyboardState.IsKeyDown(Keys.Up)) {
                movement.Z = 1;
            }
            if (keyboardState.IsKeyDown(Keys.J)) {
                movement.Y = 1;
            }
            else if(keyboardState.IsKeyDown(Keys.K)) {
                movement.Y = -1;
            }
            if (updateCount > 180) {
                Console.WriteLine(Position.X + Position.Y + Position.Z);
                updateCount = 0;
            }
            Matrix orientationMatrix = Matrix.CreateRotationY(ForwardDirection);

            Vector3 speed = Vector3.Transform(movement, orientationMatrix);
            speed *= GameConstants.Velocity;
            futurePosition = Position + speed;

            Position = futurePosition;
        }
    }
}