using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using DX = SlimDX.Direct3D9;

namespace TestShifts
{
	// Copypasted from http://www.gamedev.ru/code/articles/slimdx_d3d9

	public struct ViewportParams
	{
		public int WindowHeight;
		public int WindowWidth;
		public bool FullScreen;
		public bool VerticalSync;
		public IntPtr ControlHandle;
		public Color4 BackColor;    //Цвет фона нашего окна
	}

	public class ViewportDX : IDisposable
	{
		//Параметры вида
		public ViewportParams Params { get; private set; }
		//Объект устройства
		public DX.Device Device { get; private set; }
		//Объект D3D
		public DX.Direct3D D3D { get; private set; }
		//Внутренние параметры, понадобится при ресайзе
		DX.PresentParameters d3dpp = new DX.PresentParameters();

		public ViewportDX(ViewportParams p)
		{
			Params = p;
			D3D = new DX.Direct3D();
			//Получаем текущий режим экрана
			DX.DisplayMode mode = D3D.GetAdapterDisplayMode(0);
			//Инициализируем параметры
			d3dpp.AutoDepthStencilFormat = DX.Format.D16;
			d3dpp.BackBufferCount = 1;
			d3dpp.BackBufferFormat = mode.Format;
			d3dpp.BackBufferHeight = p.WindowHeight;
			d3dpp.BackBufferWidth = p.WindowWidth;
			//Куда рисовать
			d3dpp.DeviceWindowHandle = p.ControlHandle;
			//Контроль вертикальной развертки
			if (p.VerticalSync)
				d3dpp.PresentationInterval = DX.PresentInterval.Default;
			else
				d3dpp.PresentationInterval = DX.PresentInterval.Immediate;
			d3dpp.SwapEffect = DX.SwapEffect.Discard;
			d3dpp.Windowed = !p.FullScreen;
			d3dpp.EnableAutoDepthStencil = true;
			if (!p.FullScreen && p.VerticalSync)
				d3dpp.SwapEffect = DX.SwapEffect.Copy;
			//Создаем девайс
			Device = new DX.Device(D3D, 0, DX.DeviceType.Hardware, p.ControlHandle, DX.CreateFlags.HardwareVertexProcessing, d3dpp);
			if (Device == null)
				throw new NullReferenceException("Не удалось инициализировать устройство!");
		}
		//Инициализация параметров устройства
		private void InitDevice()
		{
			Device.SetRenderState(DX.RenderState.ZEnable, true);
			Device.SetRenderState<DX.Cull>(DX.RenderState.CullMode, DX.Cull.None);
			Device.SetRenderState(DX.RenderState.Lighting, false);
		}

		public void Clear(Color4 backColor)
		{
			Device.Clear(DX.ClearFlags.Target | DX.ClearFlags.ZBuffer, backColor, 1.0f, 0);
		}
		public void Clear()
		{
			Clear(Params.BackColor);
		}

		public void Begin()
		{
			Device.BeginScene();
		}
		public void End()
		{
			Device.EndScene();
		}
		public void RenderToScreen()
		{
			Device.Present();
		}

		public void Dispose()
		{
			if (Device != null)
				Device.Dispose();
			D3D.Dispose();
		}

		public DX.RasterStatus GetRasterStatus()
		{
			return Device.GetRasterStatus(0);
		}
	}
}
