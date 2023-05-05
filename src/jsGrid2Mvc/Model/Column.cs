using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace jsGrid2Mvc.Model
{
    /// <summary>
    /// Object to model aGrid Column
    /// </summary>
    public class Column : IModelObject
    {
        #region Properties
        /// <summary>
        /// name of Column
        /// </summary>
        public string ControlId { get; private set; }
        /// <summary>
        /// Type of data managed by the column
        /// </summary>
        public EnumColumnTypes ColumnType{ get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Width { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSortingEnabled{ get; private set; }
        /// <summary>
        /// 
        /// </summary>
		public bool IsSearchingEnabled { get; private set; }
        /// <summary>
        /// Label to show on column header
        /// </summary>
		public string Title { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public EnumColumnAlign? Align{ get; private set; }
        /// <summary>
        /// For <see cref="ColumnTypesEnum.select"/> column, the name of collection that is used to pickup the right value
        /// </summary>
        public string RefCollectionName { get; private set; }
		/// <summary>
		/// For <see cref="ColumnTypesEnum.select"/> column, the name of field that is used to pickup the right value
		/// </summary>
		public string RefCollectionValueId { get; private set; }
		/// <summary>
		/// For <see cref="ColumnTypesEnum.select"/> column, the name of field that is used to show calue in the column
		/// </summary>
		public string RefCollectionTextField { get; private set; }
        #endregion

        #region Fields
        /// <summary>
        /// fucntion fo validate cell
        /// </summary>
        internal string _Validate { get; private set; }
        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="title"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Column(string name, string width, string title = "")
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (String.IsNullOrEmpty(width))
                throw new ArgumentNullException(nameof(width));
            this.ControlId = name;
            this.Width = width;
            this.ColumnType = EnumColumnTypes.text;
            this._Validate = "";
            this.Title = String.IsNullOrEmpty(title) ? "" : title;

            this.RefCollectionName = "";
            this.RefCollectionValueId = "";
            this.RefCollectionTextField = "";
            this.IsSearchingEnabled = true;

		}

        #region Used Types
        /// <summary>
        /// Supported types of column
        /// </summary>
        public enum EnumColumnTypes
        { 
            text,
            number,
            select,
            checkbox,
            control
        }
        /// <summary>
        /// Supported alignment
        /// </summary>
        public enum EnumColumnAlign
        { 
            left,
            center,
            right
        }
        #endregion

        /// <summary>
        /// Makes the column value required
        /// </summary>
        /// <returns></returns>
        public Column Required()
        {
            this._Validate = "required";
            return this;
        }
        /// <summary>
        /// Makes the column sortable
        /// </summary>
        /// <returns></returns>
        public Column Sortable()
        {
            this.IsSortingEnabled = true;
            return this;
        }
        /// <summary>
        /// Prevents to use current column as filter (the header does not let you to serach for values)
        /// </summary>
        /// <returns></returns>
		public Column NotSearchable()
		{
			this.IsSearchingEnabled = false;
			return this;
		}
        /// <summary>
        /// Marks the column as a control one, not data one
        /// </summary>
        /// <returns></returns>
		public Column AsControl()
        {
            this.ColumnType = EnumColumnTypes.control;
            NotSearchable();
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Column AsText()
        {
            this.ColumnType = EnumColumnTypes.text;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Column AsNumber()
        {
            this.ColumnType = EnumColumnTypes.number;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Column AsCheckBox()
        {
            this.ColumnType = EnumColumnTypes.checkbox;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="refCollectionName">collection to use to pickup value</param>
        /// <param name="valueIdField">field to use to select item from collection</param>
        /// <param name="textValueField">filed of collection item used to disply value in grid</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Column AsSelect(string refCollectionName, string valueIdField, string textValueField)
        {
            if (String.IsNullOrEmpty(refCollectionName))
                throw new ArgumentNullException(nameof(refCollectionName));
            if (String.IsNullOrEmpty(valueIdField))
                throw new ArgumentNullException(nameof(valueIdField));
            if (String.IsNullOrEmpty(textValueField))
                throw new ArgumentNullException(nameof(textValueField));

            this.ColumnType = EnumColumnTypes.select;
            this.RefCollectionName = refCollectionName;
            this.RefCollectionValueId = valueIdField;
            this.RefCollectionTextField = textValueField;
            return this;
        }
        /// <summary>
        /// Sets the horizontal alignment
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        public Column AlignOn(EnumColumnAlign align)
        {
            this.Align = align;
            return this;
        }
    }
}
