
// mINI.
// A minimal, customizable INI reader for .NET in a single abstract class.


using System;


namespace mINI
{
    public abstract class INIReader
    {
        /// <summary>
        /// Called when the current line is empty.
        /// </summary>
        protected virtual void OnEmpty() {}

        /// <summary>
        /// Called when the current line is a comment.
        /// </summary>
        /// <param name="line">Complete line, comment prefix included.</param>
        protected virtual void OnComment(String line) {}

        /// <summary>
        /// Called when the current line is a section,
        /// before reading subsections.
        /// Can be used for two main purposes:
        /// Noticing each new section and creating INI readers
        /// that don't allow subsections.
        /// </summary>
        /// <param name="section">
        /// Section path, including subsections. Example: "a/b/c/d".
        /// </param>
        protected virtual void OnSection(String section) {}

        /// <summary>
        /// Called each time a subsection is found in a section line.
        /// <para>
        /// For example, for a line such as: [a/b/c], this method
        /// will be called 3 times with the following arguments:
        /// </para>
        /// <para> OnSubSection("a", "a") </para>
        /// <para> OnSubSection("b", "a/b") </para>
        /// <para> OnSubSection("c", "a/b/c") </para>
        /// </summary>
        /// <param name="section">Section name.</param>
        /// <param name="path">Section path, including parents.</param>
        protected virtual void OnSubSection(String section, String path) {}

        /// <summary>
        /// Called when a section or a subsection name is empty.
        /// </summary>
        /// <param name="section">Section name.</param>
        /// <param name="path">Complete section path, including parents.</param>
        protected virtual void OnSectionEmpty(String section, String path) {}

        /// <summary>
        /// Called when the current line is a key=value pair.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value associated with the key.</param>
        protected virtual void OnKeyValue(String key, String value) {}

        /// <summary>
        /// Called when a key is empty in a key=value pair.
        /// </summary>
        /// <param name="value">Value associated with the key.</param>
        protected virtual void OnKeyEmpty(String value) {}

        /// <summary>
        /// Called when a value is empty in a key=value pair.
        /// </summary>
        /// <param name="key">Key specified for the value.</param>
        protected virtual void OnValueEmpty(String key) {}

        /// <summary>
        /// Try to read an empty line.
        /// </summary>
        /// <param name="line">Input line.</param>
        private Boolean ReadEmpty(String line)
        {
            if (line != String.Empty)
                return false;

            OnEmpty();
            return true;
        }

        /// <summary>
        /// Try to read a comment.
        /// </summary>
        /// <param name="line">Input line.</param>
        private Boolean ReadComment(String line)
        {
            if (!(line.StartsWith("#") || line.StartsWith(";")))
                return false;

            OnComment(line);
            return true;
        }

        /// <summary>
        /// Try to read a (possibly nested) section.
        /// </summary>
        /// <param name="line">Input line.</param>
        private Boolean ReadSection(String line)
        {
            if (!(line.StartsWith("[") && line.EndsWith("]")))
                return false;

            // trim and report that we are entering a section:
            String section = line.Substring(1, line.Length - 2).Trim();
            OnSection(section);

            String[] subsections = section.Split('/');

            // first section is special
            // no separator, name/path identical:
            String path = subsections[0].Trim();

            if (path == String.Empty)
                OnSectionEmpty(path, path);

            OnSubSection(path, path);

            // rest of sections, accumulate path:
            for (Int32 i = 1; i < subsections.Length; i++)
            {
                String subsection = subsections[i].Trim();
                path += "/" + subsection;

                if (subsection == String.Empty)
                    OnSectionEmpty(subsection, path);

                OnSubSection(subsection, path);
            }
            return true;
        }

        /// <summary>
        /// Try to read a key=value pair.
        /// </summary>
        /// <param name="line">Input line.</param>
        private Boolean ReadKeyValue(String line)
        {
            if (!line.Contains("="))
                return false;

            String[] pair = line.Split('=');
            String key = pair[0];
            String value = pair[1];

            if (key == String.Empty)
                OnKeyEmpty(value);

            if (value == String.Empty)
                OnValueEmpty(key);

            OnKeyValue(key, value);
            return true;
        }

        /// <summary>
        /// Try to read an INI line.
        /// </summary>
        /// <param name="line">Input line.</param>
        public Boolean ReadLine(String line)
        {
            String text = line.Trim();

            return ReadEmpty(line)
                || ReadComment(line)
                || ReadSection(line)
                || ReadKeyValue(line);
        }
    }
}

