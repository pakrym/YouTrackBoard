using System;
using System.Collections.Generic;
using System.Linq;

namespace YoutrackBoard
{
    internal class Project
    {

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Versions { get; set; }

        public Sprint[] Sprints
        {
            get { return Versions.Trim('[', ']').Split(new[] {' ', ','},StringSplitOptions.RemoveEmptyEntries).Select(s => new Sprint(s)).ToArray(); }
        }

        public List<Subvalue> AssigneesFullName { get; set; }
        public List<Subvalue> AssigneesLogin { get; set; }

        public Person[] People
        {
            get { return AssigneesFullName.Zip(AssigneesLogin, (n, l) => new Person(n, l)).Concat(new[] {new Person("Root","root"), }).ToArray(); }
        }
    }
}