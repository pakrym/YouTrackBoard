using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using ReactiveUI;


namespace YoutrackBoard
{
    using System.Reactive.Linq;

    using YoutrackBoard.ReviewboardApi;

    class ProjectDetailsViewModel: Screen
    {
        private Project _project;
        
        private readonly ProjectRepository _projectRepository;

        public ProjectDetailsViewModel(
            Project project, 
            Sprint sprint, 
            Func<Project, Sprint, Person, PersonDetailsViewModel> personDetailsFactory
            )
        {
            _project = project;
        
            Name = _project.Name;

            PersonDetails = new ObservableCollection<PersonDetailsViewModel>(
                _project.People.Select(p=>personDetailsFactory(project,sprint,p))
                );
        }


        public ObservableCollection<PersonDetailsViewModel> PersonDetails { get; set; }

        public string Name { get; set; }

        
    }
}