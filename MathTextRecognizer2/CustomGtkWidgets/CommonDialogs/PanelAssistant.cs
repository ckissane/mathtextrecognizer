using System;
using System.Collections;

using Gtk;

using CustomGtkWidgets.CommonDialogs;


namespace CustomGtkWidgets.CommonDialogs
{

	/// <summary>
	/// Esta clase crea el asistente que se usa para guiar el proceso de creación
	/// de una nueva base de datos.
	/// <summary>
	public class PanelAssistant
	{
		
		#region Widgets de Glade
		
		[Glade.WidgetAttribute]
		private Button acceptButton;
		
		[Glade.WidgetAttribute]
		private Button backButton;		
		
		[Glade.WidgetAttribute]
		private Dialog panelAssistant;
		
		[Glade.WidgetAttribute]
		private Button nextButton;
		
		[Glade.WidgetAttribute]
		private HBox stepsHBox;
		
		#endregion Widgets de Glade
		
		#region Atributos
		
		private int panelIdx;	
				
		private ArrayList steps;
		
		#endregion Atributos
		
		#region Constructores
		
		/// <summary>
		/// Crea una instancia de <c>PanelAssistant</c>.
		/// </summary>
		/// <param name = "parent">
		/// La ventana desde la que se lanza el asistente.
		/// </param>
		/// <param name = "title">
		/// El título de la ventana del asistente.
		/// </param>
		public PanelAssistant(Window parent, string title)
		{
			Glade.XML gxml =
				new Glade.XML(null,"gui.glade","panelAssistant",null);
				
			gxml.Autoconnect(this);
			
			panelAssistant.Title = title;
			
			if(parent != null)
				panelAssistant.Icon = parent.Icon;
			
			panelAssistant.TransientFor = parent;
			panelAssistant.Modal = true;			
			
			panelIdx = 0;			
			
			steps = new ArrayList();
		}
		
		#endregion Constructores
		
		#region Propiedades
		
		/// <summary>
		/// Permite recuperar la ventana en la que se muestra el 
		/// asistente.
		/// </summary>
		public Window Window
		{
			get
			{
				return panelAssistant;
			}
		}
		
		#endregion Propiedades
		
		#region Metodos publicos
		
		public void AddStep(PanelAssistantStep step)
		{
			// We will add a step to the dialog;
			
			steps.Add(step);
			stepsHBox.Add(step.StepWidget);
			step.Hide();
			
			SetIndex(0);					
		}
		
		public void Destroy()
		{
			this.panelAssistant.Destroy();
		}
		
		/// <summary>
		/// Este método permite mostrar el diálogo, esperando a que el asistente responda
		/// un resultado.
		/// </summary>
		public ResponseType Run()
		{
			// Así no salimos si pulsamos un botón del asistente que no sea cancelar
			// o aceptar.
			ResponseType res = ResponseType.None;
			
			while(res == ResponseType.None)
				res= (ResponseType)panelAssistant.Run();
			
			return res;
		}
		
		#endregion Metodos publicos
		
		#region Metodos privados
		
		
		
		private void OnAcceptButtonClicked(object sender, EventArgs a)
		{
			
		}
		
		private void OnBackButtonClicked(object sender, EventArgs a)
		{
			SetIndex(panelIdx - 1);
			
			// Porque sino se cierra el dialogo
			panelAssistant.Respond(ResponseType.None);
		}
		
		private void OnCancelButtonClicked(object sender, EventArgs a)
		{
			
		}
		
		private void OnNextButtonClicked(object sender, EventArgs a)
		{			
			SetIndex(panelIdx + 1);// nos vamos al siguiente panel.			
			panelAssistant.Respond(ResponseType.None);// sino se cierra el dialogo
		}
		
		private void SetIndex(int idx)
		{
			if(idx > panelIdx 
				&& (steps[panelIdx] as PanelAssistantStep).HasErrors())
			{
				OkDialog.Show(
					panelAssistant,
					MessageType.Warning,
					"Antes de continuar, debe tener en cuenta que:\n\n{0}",
					(steps[panelIdx] as PanelAssistantStep).Errors);
					
			}
			else
			{
				// Activamos o desactivamos los botones segun corresponda.
				panelIdx = idx;
				backButton.Sensitive = idx > 0;
				nextButton.Sensitive = idx < steps.Count -1;
				
				acceptButton.Sensitive = idx == steps.Count -1;
				
				// Mostramos los paneles adecuados.
				int i = 0;
				foreach(PanelAssistantStep step in steps)
				{
					if(i == panelIdx)
						step.Show();
					else
						step.Hide();
						
					i++;
				}
			
			}
		}
		
		#endregion Metodos privados
	}

} 