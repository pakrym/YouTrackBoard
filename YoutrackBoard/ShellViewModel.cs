using System;
using Caliburn.Micro;
using ReactiveUI;


namespace YoutrackBoard
{
    internal class ShellViewModel: Conductor<Screen>.Collection.OneActive, IShell
    {
        private readonly Func<ProjectSelectorViewModel> _projectSelectorFactory;

        public ShellViewModel(
            Func<ProjectSelectorViewModel> projectSelectorFactory
            )
        {
            _projectSelectorFactory = projectSelectorFactory;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Navigate(_projectSelectorFactory());
        }

        public void Back()
        {
            if (Items.Count > 1)
            {
                Items.RemoveAt(Items.Count - 1);
            }
            ActivateItem(Items[Items.Count-1]);
        }

        public void Navigate(Screen screen)
        {
            ActivateItem(screen);
        }
    }
}