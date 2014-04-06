using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace YoutrackBoard
{
    using System.Collections.Generic;

    using ReactiveUI;

    internal class SprintSelectorViewModel: ReactiveObject
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

            _projectRepository.GetSprintsObservable(_project).ToProperty(this, t => t.AllSprints, out allSprints);
        }
        
        private ObservableAsPropertyHelper<List<Sprint>> allSprints;

        public List<Sprint> AllSprints 
        { 
            get
            {
                return allSprints.Value;
            } 
        }

        public void Select(Sprint sprint)
        {
            _shell.Navigate(_projectDetailsFactory(_project,sprint));
        }
    }
}