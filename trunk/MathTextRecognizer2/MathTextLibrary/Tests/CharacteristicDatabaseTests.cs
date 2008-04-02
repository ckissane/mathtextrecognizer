// CharacteristicDatabaseTests.cs created with MonoDevelop
// User: luis at 17:44 27/03/2008

using System;
using NUnit.Framework;

using MathTextLibrary.Symbol;

using MathTextLibrary.Databases;
using MathTextLibrary.Databases.Characteristic;

namespace MathTextLibrary
{
	
	
	[TestFixture()]
	public class CharacteristicDatabaseTests
	{
		
		/// <summary>
		/// Probamos que añadir un simbolo con conteniddo igual a un nodo
		/// no lo añade realmente.
		/// </summary>
		[Test()]
		public void RepeatedSymbolNotAdded()
		{
			CharacteristicDatabase database = new CharacteristicDatabase();
			
			database.CharacteristicNode = new CharacteristicNode();
			
			try {
				database.CharacteristicNode.AddSymbol(new MathSymbol("hola",MathSymbolType.Operator));
			}
			catch(DuplicateSymbolException e)
			{
				
			}
			int count1 = database.CharacteristicNode.Symbols.Count;
			
			bool exception=false;
			
			try {
				database.CharacteristicNode.AddSymbol(new MathSymbol("hola",MathSymbolType.Operator));
			}
			catch(DuplicateSymbolException e)
			{
				exception=true;
			}
			int count2 = database.CharacteristicNode.Symbols.Count;count2 = database.CharacteristicNode.Symbols.Count;
			
			
				
			
			Assert.IsTrue(exception,"No se lanzo la excepcion del elemento duplicado");
		}
	}
}