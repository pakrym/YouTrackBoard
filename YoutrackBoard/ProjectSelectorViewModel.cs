using System;
using System.Collections.Generic;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Caliburn.Micro;
using ReactiveUI;


namespace YoutrackBoard
{
    internal class ProjectSelectorViewModel : ReactiveObject, IRefreshable
    {
        private readonly ProjectRepository _projectRepository;
        private readonly Func<Project, SprintSelectorViewModel> _sprintSelectorFactory;
        private readonly IShell _shell;

        private ObservableAsPropertyHelper<List<Project>> _allProjects;
        

        public ProjectSelectorViewModel(
            ProjectRepository projectRepository,
            Func<Project,SprintSelectorViewModel> sprintSelectorFactory,
            IShell shell
            
            )
        {
            _projectRepository = projectRepository;
            _sprintSelectorFactory = sprintSelectorFactory;
            _shell = shell;

            _projectRepository.GetAllObservable().
                ToProperty(this, t => t.AllProjects, out _allProjects);
        
        }

        
        public List<Project> AllProjects
        {
            get { return _allProjects.Value; }
        }

        public void Select(Project p)
        {
            _shell.Navigate(_sprintSelectorFactory(p));   
        }

        public void Refresh()
        {
            _projectRepository.RefreshData();
        }
    }
}