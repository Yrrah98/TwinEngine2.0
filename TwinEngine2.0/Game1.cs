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
    private LevelSystem _levelSystem;
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
        _world.AddSystem(new MovementSystem(        // Order: 100
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height
        ));
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

        // Setup level system and load the first level
        _levelSystem = new LevelSystem(_world, _whiteTexture);
        _levelSystem.LoadLevel("./Content/Levels/level_03.json");
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
