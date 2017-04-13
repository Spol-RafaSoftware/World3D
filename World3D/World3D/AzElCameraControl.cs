using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Input;

namespace World3D
{
	public class AzElCameraControl
	{
		AzElCamera camera;
		GameWindow game;
		bool leftMouseDown = false;
		bool[] moveKeyDown = new bool[6];
		bool shiftKeyDown = false;
		double speed = 1.0;

		public double MoveSpeed { get { return speed; } set { speed = value; } }
       

		public AzElCameraControl(GameWindow game, AzElCamera camera)
		{
			this.game = game;
			this.camera = camera;
			this.game.Mouse.Move += new EventHandler<MouseMoveEventArgs>(Mouse_Move);
			this.game.Mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(Mouse_WheelChanged);
			this.game.Mouse.ButtonDown += (o, e) => { if (e.Button == MouseButton.Left) leftMouseDown = e.IsPressed; };
			this.game.Mouse.ButtonUp += (o, e) => { if (e.Button == MouseButton.Left) leftMouseDown = e.IsPressed; };
			this.game.MouseLeave += (o, e) => leftMouseDown = false;
			this.game.Keyboard.KeyDown += (o, e) => UpdateKeyState(e.Key, true);
			this.game.Keyboard.KeyUp += (o, e) => UpdateKeyState(e.Key, false);
		}


		private void UpdateKeyState(Key key, bool isDown)
		{
			switch (key)
			{
				case Key.A:
					moveKeyDown[0] = isDown;
					break;
				case Key.D:
					moveKeyDown[1] = isDown;
					break;
				case Key.S:
					moveKeyDown[2] = isDown;
					break;
				case Key.W:
					moveKeyDown[3] = isDown;
					break;
				case Key.Q:
					moveKeyDown[4] = isDown;
					break;
				case Key.E:
					moveKeyDown[5] = isDown;
					break;
				case Key.ShiftLeft:
				case Key.ShiftRight:
					shiftKeyDown = isDown;
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// Updates state.
		/// </summary>
		/// <param name="time">Seconds since last update. Get it from FrameEventArgs</param>
		public void Update(double time)
		{
			double d = time*speed;
			if (shiftKeyDown) d *= 100;
			double sideways = 0;
			if (moveKeyDown[0]) sideways -= d;
			if (moveKeyDown[1]) sideways += d;
			double forwards = 0;
			if (moveKeyDown[2]) forwards -= d;
			if (moveKeyDown[3]) forwards += d;
			double upwards = 0;
			if (moveKeyDown[4]) upwards -= d;
			if (moveKeyDown[5]) upwards += d;
			camera.MoveSideways(sideways);
			camera.MoveForwards(forwards);
			camera.MoveUpwards(upwards);
		}


		void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
		{
				double d = e.DeltaPrecise;
				d = Math.Pow(2, d);
				speed *= d;
			
		}

		void Mouse_Move(object sender, MouseMoveEventArgs e)
		{
			if (leftMouseDown)
			{
				double x = e.XDelta;
				double y = e.YDelta;
				camera.Elevation -= y * 0.01;
				camera.Azimuth += x * 0.01;
			}
		}

        public void SetPosition(Vector3 pos, Vector2 azEl)
        {
            camera.Elevation = azEl.Y;
            camera.Azimuth = azEl.X;
            camera.Eye = pos;
        }

	}
}
