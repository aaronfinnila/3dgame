using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3dgame;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private GameObject ground;
    private Box[] boxes;
    private Camera gameCamera;
    private Player player;
    private KeyboardState currentKeyboardState = new KeyboardState();
    private MouseState currentMouseState = new MouseState();
    private MouseState previousMouseState = new MouseState();
    private int screenCenterX;
    private int screenCenterY;
    private float cameraYaw;
    private float cameraPitch;
    private double logTimer = 0d;

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
        boxes = new Box[1];
        boxes[0] = new Box();
        boxes[0].LoadContent(Content, "Models/box");
        boxes[0].Position = new Vector3(0, 0, 10);
    }

    protected override void Update(GameTime gameTime) {
        if (!IsActive) {
            base.Update(gameTime);
            return;
        }
        currentKeyboardState = Keyboard.GetState();
        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();
        float maxPitchNegative = MathHelper.ToRadians(-85f);
        float maxPitchPositive = MathHelper.ToRadians(85f);

        float deltaX = currentMouseState.X - screenCenterX;
        float deltaY = currentMouseState.Y - screenCenterY;
        cameraYaw -= deltaX * GameConstants.sensitivity;
        float pitchDelta = -deltaY * GameConstants.sensitivity;
        float newPitch = cameraPitch + pitchDelta;

        if (!((cameraPitch >= maxPitchPositive && pitchDelta > 0f) || (cameraPitch <= maxPitchNegative && pitchDelta < 0f))) {
            cameraPitch = newPitch;
        }
        
        cameraPitch = MathHelper.Clamp(cameraPitch, maxPitchNegative, maxPitchPositive);

        Mouse.SetPosition(screenCenterX, screenCenterY);
        
        player.Update(currentKeyboardState, cameraYaw);

        gameCamera.Update(cameraYaw, cameraPitch, player.Position, _graphics.GraphicsDevice.Viewport.AspectRatio);

        if (currentKeyboardState.IsKeyDown(Keys.Escape)) {
            Exit();
        }

        logTimer += gameTime.ElapsedGameTime.TotalSeconds;

        if (logTimer >= 3) {
            Console.WriteLine("cameraPitch: " + cameraPitch + " cameraYaw: " + cameraYaw);
            logTimer = 0;
        }
        
        base.Update(gameTime);
    }

    private void DrawTerrain(Model model) {
        foreach (ModelMesh mesh in model.Meshes) {
            foreach (BasicEffect effect in mesh.Effects) {
                effect.EnableDefaultLighting();
                effect.PreferPerPixelLighting = true;
                effect.World = Matrix.Identity;

                // use the matrices provided by the game camera
                effect.View = gameCamera.ViewMatrix;
                effect.Projection = gameCamera.ProjectionMatrix;
            }
            mesh.Draw();
        }
    }
    
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);
        DrawTerrain(ground.Model);
        foreach (Box box in boxes) {
            box.Draw(gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);
        }
        
        base.Draw(gameTime);
    }
}