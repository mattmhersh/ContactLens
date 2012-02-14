// Copyright (c) 2003, Michael Mayer
// See License.txt that should have been included with this source file.
// or see http://www.mag37.com/projects/Printing/

using System;
using System.Collections;

namespace ReportPrinting
{
	/// <summary>
	/// This abstract class defines a container of sections.
	/// </summary>
    public abstract class SectionContainer : ReportSection
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SectionContainer()
        {
        }

        /// <summary>
        /// ArrayList of sections in this container.
        /// </summary>
        protected ArrayList sections = new ArrayList();

        /// <summary>
        /// Index to the current section
        /// </summary>
        protected int sectionIndex;

        /// <summary>
        /// Gets the Current ReportSection
        /// </summary>
        protected ReportSection CurrentSection
        {
            get
            {
                if (this.sectionIndex < this.sections.Count)
                {
                    return (ReportSection) this.sections[this.sectionIndex]; 
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Add a section object to the list of sections
        /// Each section object should be a new instance of ReportSection
        /// </summary>
        /// <param name="section">The section info to add</param>
        /// <returns>The number of sections</returns>
        public virtual int AddSection (ReportSection section)
        {
            return this.sections.Add(section);
        }

        /// <summary>
        /// Removes a section from the document
        /// </summary>
        /// <param name="index">Index of the section to remove</param>
        public virtual void RemoveSection(int index)
        {
            this.sections.RemoveAt(index);
        }

        /// <summary>
        /// Gets the section at the specified index
        /// </summary>
        /// <param name="index">Index of a section</param>
        /// <returns>A ReportSection object</returns>
        public virtual ReportSection GetSection(int index)
        {
            return (ReportSection) this.sections[index];
        }

        /// <summary>
        /// The number of sections in this document.
        /// </summary>
        public virtual int SectionCount
        {
            get { return this.sections.Count; }
        }

        /// <summary>
        /// Clears all sections from the document.
        /// Printing a document with 0 sections is just fine,
        /// a little weird, but technically fine.
        /// </summary>
        public virtual void ClearSections()
        {
            this.sections.Clear();
        }

        /// <summary>
        /// Resets the size of the section, that is, it enforces
        /// that a call to CalcSize() will actully have an effect,
        /// and not just use a stored value.
        /// </summary>
        public override void ResetSize()
        {
            base.ResetSize();
            if (CurrentSection != null)
            {
                CurrentSection.ResetSize();
            }
        }

        /// <summary>
        /// Resets the entire section, useful at the very beginning
        /// of a print (before Graphics is even known)
        /// to reset startedPrinting.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            foreach (ReportSection section in this.sections)
            {
                section.Reset();
            }
        }



	}
}
