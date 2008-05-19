// ExpressionItemOptionsDialog.cs created with MonoDevelop
// User: luis at 16:06 19/05/2008

using System;
using System.Collections.Generic;

using Gtk;
using Glade;

using MathTextLibrary.Analisys;

namespace MathTextRecognizer.SyntacticalRulesManager
{
	
	/// <summary>
	/// This class implements a dialog to show the options of a expression's
	/// items.
	/// </summary>
	public class ExpressionItemOptionsDialog : IExpressionItemContainer
	{
		
#region Glade widgets
		
		[Widget]
		private Dialog expressionItemOptionsDialog = null;
		
		[Widget]
		private ScrolledWindow itemOpRelatedItemsScroller = null;
		
		[Widget]
		private VBox itemOpRelatedItemsBox = null;
		
		[Widget]
		private CheckButton itemOpForceSearchCheck = null;
		
		[Widget]
		private ComboBox itemOpModifierCombo = null;
		
		[Widget]
		private Entry itemOpFormatEntry = null;
		
		[Widget]
		private Alignment itemOpFormatAlignment = null;
		
		[Widget]
		private Frame itemOpRelatedItemsFrame = null;
		
#endregion Glade widgets
		
#region Fields
		
		AddSubItemMenu addItemMenu;
		
#endregion Fields
		
		public ExpressionItemOptionsDialog(Window parent, Type expressionType)
		{
			XML gladeXml = new XML("mathtextrecognizer.glade",
			                       "expressionItemOptionsDialog");
			
			gladeXml.Autoconnect(this);
			
			addItemMenu = new AddSubItemMenu(this);
			
			this.expressionItemOptionsDialog.TransientFor = parent;
			
			InitializeWidgets(expressionType);
		
			
		}
		
#region Properties
		
		/// <value>
		/// Contains the number of related items.
		/// </value>
		public int ItemCount 
		{
			get 
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Allows the retrieve the info about a widget's position in 
		/// the related items box.
		/// </summary>
		/// <param name="widget">
		/// A <see cref="ExpressionItemWidget"/>
		/// </param>
		public Gtk.Box.BoxChild this[Widget w] 
		{
			get 
			{
				throw new NotImplementedException();
			}
			set 
			{
				throw new NotImplementedException();
			}
		}
		
		/// <value>
		/// Contains the dialog's window.
		/// </value>
		public Window Window
		{
			get
			{
				return this.expressionItemOptionsDialog;
			}
		}
		
		/// <value>
		/// Contains the items options.
		/// </value>
		public ExpressionItemOptions Options
		{
			get
			{
				ExpressionItemOptions options = new ExpressionItemOptions();
				options.ForceCheck = itemOpForceSearchCheck.Active;
				options.Modifier =
					(ExpressionItemModifier)(itemOpModifierCombo.Active);
				
				options.FormatString = itemOpFormatEntry.Text.Trim();
				
				foreach (RelatedItemWidget childWidget in itemOpRelatedItemsBox.Children) 
				{
					options.RelatedItems.Add(childWidget.ExpressionItem);
				}
				
				return options;
			}
			
			set
			{
				itemOpForceSearchCheck.Active =  value.ForceCheck;
				itemOpFormatEntry.Text =  value.FormatString;
				
				itemOpModifierCombo.Active = (int)(value.Modifier);
				
				
			}
		}
		
#endregion Properties
		
#region Public methods

		/// <summary>
		/// Adds an item to the container.
		/// </summary>
		/// <param name="widget">
		/// A <see cref="ExpressionItemWidget"/>
		/// </param>
		public void AddItem (ExpressionItemWidget widget)
		{
			
		}

		/// <summary>
		/// Removes an item from the container.
		/// </summary>
		/// <param name="widget">
		/// A <see cref="ExpressionItemWidget"/>
		/// </param>
		public void RemoveItem (ExpressionItemWidget widget)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Moves an item towards the beggining of the container.
		/// </summary>
		/// <param name="widget">
		/// A <see cref="ExpressionItemWidget"/>
		/// </param>
		public void MoveItemBackwards (ExpressionItemWidget widget)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Moves an item towards the end of the container.
		/// </summary>
		/// <param name="widget">
		/// A <see cref="ExpressionItemWidget"/>
		/// </param>
		public void MoveItemFordwards (ExpressionItemWidget widget)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Shows the dialog and waits for its response.
		/// </summary>
		/// <returns>
		/// A <see cref="ResponseType"/>
		/// </returns>
		public ResponseType Show()
		{
			ResponseType res;
			// We wait for a valid response.
			do
			{
				res = (ResponseType)(this.expressionItemOptionsDialog.Run() );
				
			}
			while(res == ResponseType.None);
			
			return res;
		}
		
		/// <summary>
		/// Hides the dialog, and frees its resources.
		/// </summary>
		public void Destroy()
		{
			expressionItemOptionsDialog.Destroy();
		}

#endregion Public methods
		
#region Non-public methods
		
		private void InitializeWidgets(Type expressionType)
		{
			
			
			this.itemOpModifierCombo.Active = 0;
			
			bool isToken = expressionType == typeof(ExpressionTokenItem);
			
			itemOpForceSearchCheck.Visible = isToken;
			itemOpFormatAlignment.Visible =  isToken;
			itemOpRelatedItemsFrame.Visible =  isToken;
		
		}
		
#endregion Non-public methods
	}
	
	/// <summary>
	/// This class implements a way for the dialog to encapsulate the item's 
	/// options.
	/// </summary>
	public class ExpressionItemOptions
	{
		public ExpressionItemModifier Modifier;
		public bool ForceCheck;
		public List<ExpressionItem> RelatedItems;
		public string FormatString;	
				
		public ExpressionItemOptions()
		{
			RelatedItems = new List<ExpressionItem>();
		}
		
	}
	

		
}