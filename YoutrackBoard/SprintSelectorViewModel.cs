using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace YoutrackBoard
{
    internal class SprintSelectorViewModel: Screen
    {
        
        private readonly Project _project;
        private readonly Func<Project, Sprint, ProjectDetailsViewModel> _projectDetailsFactory;
        private readonly ProjectRepository _projectRepository;
        private readonly IShell _shell;

        public SprintSelectorViewModel(Project project,
            Func<Project,Sprint,ProjectDetailsViewModel> projectDetailsFactory,
            ProjectRepository projectRepository,
            IShell shell)
        {
            _project = project;
            _projectDetailsFactory = projectDetailsFactory;
            _projectRepository = projectRepository;
            _shell = shell;
            AllSprints = new ObservableCollection<Sprint>();
            
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoadData();
        }


        private async void LoadData()
        {
            var result = await _projectRepository.GetSprints(_project);
            foreach (var sprint in result)
            {
                AllSprints.Add(sprint);
            }

        }

        public ObservableCollection<Sprint> AllSprints { get; private set; }

        public void Select(Sprint sprint)
        {
            _shell.Navigate(_projectDetailsFactory(_project,sprint));
        }
    }
}