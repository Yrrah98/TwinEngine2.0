using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Game.Components;
using TwinEngine2._0.Game.Systems;
using TwinEngine2._0.Game;

namespace TwinEngine2._0;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private World _world;
    private RenderSystem _renderSystem;
    private Texture2D _whiteTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _world = new World();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a 1x1 white texture (shared resource)
        _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
        _whiteTexture.SetData(new[] { Color.White });

        // Setup systems (automatically ordered by ExecutionOrder)
        _world.AddSystem(new InputSystem());        // Order: 0 (first)
        _world.AddSystem(new MovementSystem());     // Order: 100
        _world.AddSystem(new PhysicsSystem(         // Order: 150
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height
        ));

        var collisionSystem = new CollisionSystem(  // Order: 200
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height
        );
        _world.AddSystem(collisionSystem);

        _renderSystem = new RenderSystem(_spriteBatch, _whiteTexture);
        _renderSystem.SetCollisionSystem(collisionSystem);
        _world.AddSystem(_renderSystem);            // Order: 1000 (last)

        // Create entities
        EntityFactory.CreatePlayer(_world, new Vector2(100, 100), _whiteTexture);

        // Cluster 1: Very dense cluster in top-left sub-quadrant (0-200, 0-120)
        // This will force multiple levels of subdivision
        EntityFactory.CreateNPC(_world, new Vector2(50, 50), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(65, 52), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(80, 54), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(95, 56), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(110, 58), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(125, 60), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(140, 62), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(155, 64), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(170, 66), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(52, 75), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(67, 77), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(82, 79), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(97, 81), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(112, 83), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(127, 85), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(142, 87), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(157, 89), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(172, 91), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(55, 100), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(70, 102), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(85, 104), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(100, 106), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(115, 108), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(130, 110), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(145, 112), _whiteTexture);

        // Cluster 2: Top-right area
        EntityFactory.CreateNPC(_world, new Vector2(500, 60), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(520, 70), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(540, 80), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(560, 90), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(510, 100), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(530, 110), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(550, 120), _whiteTexture);

        // Cluster 3: Bottom area
        EntityFactory.CreateNPC(_world, new Vector2(200, 350), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(220, 360), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(240, 370), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(260, 350), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(280, 360), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(300, 370), _whiteTexture);
        EntityFactory.CreateNPC(_world, new Vector2(320, 380), _whiteTexture);

        // Scattered NPCs
        EntityFactory.CreateNPC(_world, new Vector2(350, 200), _whiteTexture);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _world.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _renderSystem.Draw();

        base.Draw(gameTime);
    }
}
