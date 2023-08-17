using ProjectsAPI.Interfaces;
using ProjectsAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Generic
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectsRepo Projects { get; }
    }
}
