using BiddaNiketon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiddaNiketon.DAL
{
    public class UnitOfWork : IDisposable
    {
        private SchoolContext context = new SchoolContext();
        private GerneralRepository<Student> studentRepository;
        private GerneralRepository<Course> courseRepository;

        public GerneralRepository<Student> StudentRepository
        {
            get
            {
                if(studentRepository == null)
                {
                    this.studentRepository = new GerneralRepository<Student>(context); 
                }
                return studentRepository;
            }
            
        }

        public GerneralRepository<Course> CourseRepository
        {
            get
            {
                if(courseRepository == null)
                {
                    this.courseRepository = new GerneralRepository<Course>(context); 
                }
                return courseRepository; 
            }
        }

        public void Save()
        {
            context.SaveChanges(); 
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}