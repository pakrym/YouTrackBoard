using System;
using Caliburn.Micro;
using ReactiveUI;


namespace YoutrackBoard
{
    using System.Windows;

    internal class ShellViewModel : Conductor<object>.Collection.OneActive, IShell
    {
        private readonly Func<ProjectSelectorViewModel> _projectSelectorFactory;
        private object flyoutItem;

        public ShellViewModel(
            Func<ProjectSelectorViewModel> projectSelectorFactory
            )
        {
            _projectSelectorFactory = projectSelectorFactory;
        }

        public Visibility FlyoutVisibility
        {
            get
            {
                return FlyoutItem != null? Visibility.Visible: Visibility.Collapsed;
            }
        }
        public object FlyoutItem
        {
            get
            {
                return this.flyoutItem;
            }
            set
            {
                if (Equals(value, this.flyoutItem))
                {
                    return;
                }
                this.flyoutItem = value;
                this.NotifyOfPropertyChange(() => this.FlyoutItem);
                this.NotifyOfPropertyChange(() => this.FlyoutVisibility);
            }
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

        public void ShowFlyout(object flyout)
        {
            FlyoutItem = flyout;
        }

        public void Navigate(object screen)
        {
            ActivateItem(screen);
        }

        public void RefreshData()
        {
            var refreshable = this.ActiveItem as IRefreshable;
            if (refreshable != null)
            {
                refreshable.Refresh();
            }
        }
    }
}