using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3dgame;

public class Game1 : Game {
    private GraphicsDeviceManager graphics;
    private GameObject ground;
    private GameObject house;
    private Camera gameCamera;
    private GameObject boundingSphere;
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
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.ApplyChanges();
        IsMouseVisible = false;
    }

    protected override void Initialize() {
        ground = new GameObject();
        house = new GameObject();
        gameCamera = new Camera();
        boundingSphere = new GameObject();
        player = new Player();
        screenCenterX = GraphicsDevice.Viewport.Width/2;
        screenCenterY = GraphicsDevice.Viewport.Height/2;
        Mouse.SetPosition(screenCenterX, screenCenterY);
        previousMouseState = Mouse.GetState();

        base.Initialize();
    }

    protected override void LoadContent() {
        ground.Model = Content.Load<Model>("Models/ground");
        house.Model = Content.Load<Model>("Models/house");
        boundingSphere.Model = Content.Load<Model>("Models/sphere1uR");
        house.BoundingBox = house.CalculateBoundingBox();
        player.Model = Content.Load<Model>("Models/player");
        player.CalculateBoundingSphere();
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
        
        player.Update(currentKeyboardState, cameraYaw, house.BoundingBox);

        gameCamera.Update(cameraYaw, cameraPitch, player.Position, graphics.GraphicsDevice.Viewport.AspectRatio);

        if (currentKeyboardState.IsKeyDown(Keys.Escape)) {
            Exit();
        }

        logTimer += gameTime.ElapsedGameTime.TotalSeconds;

        if (logTimer >= 3) {
            Console.WriteLine("posX: " + player.Position.X + " posY: " + player.Position.Y + " posZ: " + player.Position.Z);
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

    private void DrawTextures(Model model) {
        foreach (ModelMesh mesh in model.Meshes) {
            foreach (BasicEffect effect in mesh.Effects) {
                effect.EnableDefaultLighting();
                effect.PreferPerPixelLighting = true;
                effect.TextureEnabled = true;
                effect.World = Matrix.Identity;

                // use the matrices provided by the game camera
                effect.View = gameCamera.ViewMatrix;
                effect.Projection = gameCamera.ProjectionMatrix;
            }
            mesh.Draw();
        }
    }
    
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.WhiteSmoke);
        DrawTerrain(ground.Model);
        DrawTextures(house.Model);
        ChangeRasterizerState(FillMode.WireFrame);
        house.DrawBoundingSphere(gameCamera.ViewMatrix, gameCamera.ProjectionMatrix, boundingSphere);
        ChangeRasterizerState(FillMode.Solid);
        
        base.Draw(gameTime);
    }

    private RasterizerState ChangeRasterizerState(FillMode fillmode, CullMode cullMode = CullMode.None) {
        RasterizerState rasterizerState = new RasterizerState() { 
            FillMode = fillmode,
            CullMode = cullMode 
        };
        graphics.GraphicsDevice.RasterizerState = rasterizerState;
        return rasterizerState;
    }
}