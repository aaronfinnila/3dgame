using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3dgame;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private GameObject ground;
    private Camera gameCamera;
    private Player player;
    private KeyboardState currentKeyboardState = new KeyboardState();
    private MouseState currentMouseState = new MouseState();
    private MouseState previousMouseState = new MouseState();
    private int screenCenterX;
    private int screenCenterY;
    private float cameraYaw;
    private float cameraPitch;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.ApplyChanges();
        IsMouseVisible = false;
    }

    protected override void Initialize() {
        ground = new GameObject();
        gameCamera = new Camera();
        player = new Player();
        screenCenterX = GraphicsDevice.Viewport.Width/2;
        screenCenterY = GraphicsDevice.Viewport.Height/2;
        Mouse.SetPosition(screenCenterX, screenCenterY);
        previousMouseState = Mouse.GetState();

        base.Initialize();
    }

    protected override void LoadContent() {
        ground.Model = Content.Load<Model>("Models/ground");
    }

    protected override void Update(GameTime gameTime) {
        currentKeyboardState = Keyboard.GetState();
        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();

        float deltaX = currentMouseState.X - screenCenterX;
        float deltaY = currentMouseState.Y - screenCenterY;
        cameraYaw += deltaX * 0.01f;
        cameraPitch += deltaY * 0.01f;

        Mouse.SetPosition(screenCenterX, screenCenterY);
        
        player.Update(currentKeyboardState);

        gameCamera.Update(cameraYaw, cameraPitch, player.Position, _graphics.GraphicsDevice.Viewport.AspectRatio);

        if (currentKeyboardState.IsKeyDown(Keys.Escape)) {
            Exit();
        }
        
        base.Update(gameTime);
    }

    private void DrawTerrain(Model model) {
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.PreferPerPixelLighting = true;
                effect.World = Matrix.Identity;

                // Use the matrices provided by the game camera
                effect.View = gameCamera.ViewMatrix;
                effect.Projection = gameCamera.ProjectionMatrix;
            }
            mesh.Draw();
        }
    }
    
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);
        DrawTerrain(ground.Model);
        base.Draw(gameTime);
    }

}
