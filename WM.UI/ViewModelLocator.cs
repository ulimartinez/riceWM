using WM.UI.ViewModels;
using SimpleInjector;

namespace WM.UI
{
    public class ViewModelLocator
    {
        private Container container;

        public ViewModelLocator()
        {
            this.container = Program.Bootstrap();
        }

        public BarViewModel BarViewModel 
        {
            get { return this.container.GetInstance<BarViewModel>(); }
        }
    }
}
