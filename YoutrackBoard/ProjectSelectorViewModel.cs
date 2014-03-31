using System;
using System.Collections.Generic;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Caliburn.Micro;
using ReactiveUI;


namespace YoutrackBoard
{
    internal class ProjectSelectorViewModel : Screen
    {
        private readonly ProjectRepository _projectRepository;
        private readonly Func<Project, SprintSelectorViewModel> _sprintSelectorFactory;
        private readonly IShell _shell;
        private List<Project> _allProjects;
        

        public ProjectSelectorViewModel(
            ProjectRepository projectRepository,
            Func<Project,SprintSelectorViewModel> sprintSelectorFactory,
            IShell shell
            
            )
        {
            _projectRepository = projectRepository;
            _sprintSelectorFactory = sprintSelectorFactory;
            _shell = shell;
        
        }

        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoadData();
        }

        private async void LoadData()
        {
            AllProjects = await _projectRepository.GetAll();
        }

        public List<Project> AllProjects
        {
            get { return _allProjects; }
            set
            {
                if (Equals(value, _allProjects)) return;
                _allProjects = value;
                NotifyOfPropertyChange(() => AllProjects);
            }
        }

        public void Select(Project p)
        {
         _shell.Navigate(_sprintSelectorFactory(p));   
        }
    }
}