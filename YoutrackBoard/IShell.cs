using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace YoutrackBoard
{
    interface IShell
    {
        void Back();
        void Navigate(object screen);

        void ShowFlyout(object flyout);
    }
}
