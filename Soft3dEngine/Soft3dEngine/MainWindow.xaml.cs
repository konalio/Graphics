using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharpDX;
using Soft3dEngine.Core;

namespace Soft3dEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private Device _device;
        private Mesh[] _meshes;
        readonly Camera _camera = new Camera();

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var bmp = new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgra32, null);

            _device = new Device(bmp);

            // Our XAML Image control
            FrontBuffer.Source = bmp;

            _meshes = Utils.Utilities.LoadJSONFileAsync("monkey.babylon");

            _camera.Position = new Vector3(0, 0, 10.0f);
            _camera.Target = Vector3.Zero;

            // Registering to the XAML rendering loop
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        // Rendering loop handler
        void CompositionTarget_Rendering(object sender, object e)
        {
            _device.Clear(0, 0, 0, 255);

            foreach (var mesh in _meshes)
            {
                mesh.Rotation = new Vector3(mesh.Rotation.X, mesh.Rotation.Y, mesh.Rotation.Z);
            }

            _device.Render(_camera, _meshes);
            _device.Present();
        }

    }
}
