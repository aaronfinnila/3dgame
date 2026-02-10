using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3dgame;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private GameObject ground;
    private Camera gameCamera;
    private FuelCarrier fuelCarrier;
    private KeyboardState lastKeyboardState = new KeyboardState();
    private KeyboardState currentKeyboardState = new KeyboardState();

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.ApplyChanges();
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        ground = new GameObject();
        gameCamera = new Camera();
        base.Initialize();
    }

    protected override void LoadContent() {
        ground.Model = Content.Load<Model>("Models/ground");
        fuelCarrier = new FuelCarrier();
        fuelCarrier.LoadContent(Content, "Models/fuelcarrier");
    }

    protected override void Update(GameTime gameTime) {
        lastKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();
        fuelCarrier.Update(currentKeyboardState);
        gameCamera.Update(fuelCarrier.ForwardDirection, fuelCarrier.Position, _graphics.GraphicsDevice.Viewport.AspectRatio);   
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
        fuelCarrier.Draw(gameCamera.ViewMatrix, gameCamera.ProjectionMatrix);
        base.Draw(gameTime);
    }

}
