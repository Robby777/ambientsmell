using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeModel.Representation
{
	/// <summary>
	/// Summary description for LineModel.
	/// </summary>
	public class LineModel
	{
		public bool InMultipleLineComment;
		public bool HasComment;
		public bool HasString;
		public bool HasCode;
		public int  SemiCount;
		public int  StringCount;
		public bool HasConditional;
		public bool HasIteration;
		public bool HasSwitch;
		public bool HasCase;
		public bool HasAssignment;
		private bool PendingAssignment;

		public LineModel( string line, bool inMultiLine )
		{
			CheckLineState( line, inMultiLine );
		}

		enum C
		{
			START,CODE, IN_STRING, IN_CHAR, CHAR_IGNORE, STRING_IGNORE, COMMENT_START,
			SINGLE_COMMENT, IN_MULTI, EXITING_COMMENT, MULTI_LINE_FINISH
		};
	
		protected void CheckLineState( string rawLine, bool inMultiLine )
		{
			string line = rawLine.Trim();
			C state = C.START;
			if( inMultiLine )
			{
				state = C.IN_MULTI;
			}

			StringBuilder buffer = new StringBuilder( 100 );
			int lineIndex = 0;
			foreach( char c in line )
			{
				switch( state )
				{
					case C.START:
						if ( c == '"' ) { state = C.IN_STRING; }
						else if ( c == '\'' ){ state = C.IN_CHAR;   }
						else if ( c == '/' ) { state = C.COMMENT_START;  }
						else            
						{ 
							state = C.CODE;
							buffer.Append( c );
						}
						break;
					case C.CODE:
						if ( c == '"' ) { state = C.IN_STRING; }
						else if ( c == '\'' ){ state = C.IN_CHAR;   }
						else if ( c == '/' ) { state = C.COMMENT_START;  }
						else
						{
							if( c == ';' )
								this.SemiCount++;

							if( this.PendingAssignment )
							{
								if( c != '=' )
								{
									this.HasAssignment = true;
								}
								this.PendingAssignment = false;
							}
							if( c == '=' )
								this.PendingAssignment = true;
							
							buffer.Append( c );
						}
						this.HasCode = true;
							
						break;
					case C.IN_CHAR:
						if ( c == '\\' ) { state = C.CHAR_IGNORE; }
						if ( c == '\'' ) { state = C.CODE;          }
						break;
					case C.IN_STRING:
						if ( c == '\\' ) { state = C.STRING_IGNORE; }
						if ( c == '"'  ) 
						{ 
							state = C.CODE;
							this.StringCount++; 
						}
						this.HasString = true;
						break;
					case C.CHAR_IGNORE:
						state = C.IN_CHAR;
						break;
					case C.STRING_IGNORE:
						state = C.IN_STRING;
						break;
					case C.COMMENT_START:
						if ( c == '/' ) { state = C.SINGLE_COMMENT; }
						if ( c == '*' ) { state = C.IN_MULTI;       }
						if ( c == '/' || c == '*' )
						{
							this.HasComment = true;
						}
						break;
					case C.SINGLE_COMMENT:
						break;
					case C.IN_MULTI:
						if ( c == '*' ) { state = C.EXITING_COMMENT;}
						break;
					case C.EXITING_COMMENT:
						if ( c != '*' || c != '/' )     { state = C.IN_MULTI; }
						if ( c == '/' ) { state = C.MULTI_LINE_FINISH;    }
						break;
					case C.MULTI_LINE_FINISH:
						state = C.CODE;
						break;
				}
				lineIndex++;
			}
			if( state == C.IN_MULTI || state == C.EXITING_COMMENT )
				this.InMultipleLineComment = true;

			string bufStr = buffer.ToString();
			this.HasConditional = HasKeyWord( bufStr, "if","else", "case" );
			this.HasIteration   = HasKeyWord( bufStr, "while","do","for", "foreach" );
			this.HasSwitch      = HasKeyWord( bufStr, "switch" );
			this.HasCase        = HasKeyWord( bufStr, "case" );
		}

		protected bool HasKeyWord( string line, params string[] strings )
		{
			foreach( string pattern in strings )
			{
				if( Regex.Match( line, @".*\b"+pattern+@"\b.*" ).Success )
					return true;
			}
			return false;
		}
	}
}
