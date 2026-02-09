using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3dgame;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Model _model;
    private Vector3 position = Vector3.One;
    private float zoom = 2500;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;
    private Matrix gameWorldRotation;
    float speed = 10f;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _model = Content.Load<Model>("fuelcarrier");
    }

    private void UpdateGamePad() {
    GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
    KeyboardState keyState = Keyboard.GetState();

    // Gamepad controls
    position.X += gamePadState.ThumbSticks.Left.X * speed;
    position.Y += gamePadState.ThumbSticks.Left.Y * speed;
    zoom += gamePadState.ThumbSticks.Right.Y * speed;
    rotationY += gamePadState.ThumbSticks.Right.X * speed;
    if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
    {
        rotationX += 1.0f * speed;
    }
    else if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
    {
        rotationX -= 1.0f * speed;
    }

    // Keyboard controls
    if (keyState.IsKeyDown(Keys.A)) { position.X += 1.0f * speed; }
    else if (keyState.IsKeyDown(Keys.D)) { position.X -= 1.0f * speed; }
    if (keyState.IsKeyDown(Keys.W)) { zoom += 1.0f * speed; }
    else if (keyState.IsKeyDown(Keys.S)) { zoom -= 1.0f * speed; }
    if (keyState.IsKeyDown(Keys.E)) { rotationY += 1.0f * speed; }
    else if (keyState.IsKeyDown(Keys.Q)) { rotationY -= 1.0f * speed; }

    if (keyState.IsKeyDown(Keys.Right)) { position.Y += 1.0f * speed; }
    else if (keyState.IsKeyDown(Keys.Left)) { position.Y -= 1.0f * speed; }
    if (keyState.IsKeyDown(Keys.Up)) { rotationX += 1.0f * speed; }
    else if (keyState.IsKeyDown(Keys.Down)) { rotationX -= 1.0f * speed; }

    gameWorldRotation = Matrix.CreateRotationX(MathHelper.ToRadians(rotationX)) * Matrix.CreateRotationY(MathHelper.ToRadians(rotationY));
    }

    protected override void Update(GameTime gameTime) {
        UpdateGamePad();
        base.Update(gameTime);
    }

    private void DrawModel(Model m) {
        Matrix[] transforms = new Matrix[m.Bones.Count];
        float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
        m.CopyAbsoluteBoneTransformsTo(transforms);
        Matrix projection =
            Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
            aspectRatio, 1.0f, 10000.0f);
        Matrix view = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, zoom),
            Vector3.Zero, Vector3.Up);

        foreach (ModelMesh mesh in m.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();

                effect.View = view;
                effect.Projection = projection;
                effect.World = gameWorldRotation *
                    transforms[mesh.ParentBone.Index] *
                    Matrix.CreateTranslation(position);
            }
            mesh.Draw();
        }
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        DrawModel(_model);
        base.Draw(gameTime);
    }
}
