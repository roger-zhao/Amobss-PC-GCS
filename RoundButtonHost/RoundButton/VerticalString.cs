/*
 * Created by Gary Perkin.
 * Date: 11/11/2004
 * Time: 11:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace GaryPerkin.Utils.Text
{

	public class VerticalString
		{
		
		private enum esaType 
		{
			Pre, 
			Post,
			Either
		}
		
		private double textSpread;
			
		public double TextSpread
		{
			get
			{
				return textSpread;		
			}
			
			set
			{
				textSpread = value;
			}
		}
		
		[Description("VerticalString Constructor")]
		public VerticalString()
		{
			textSpread = .75F;
		}
		
		[Description("Draw Method 1 - draw string in top left of rectangle."+
		             "Calls Method 3")]
		public void Draw(Graphics g, string text, Font font, Brush brush, Rectangle stringRect)
		{
			this.Draw(g, text, font, brush, 
			          stringRect.X, 
			          stringRect.Y);	
		}
		
		[Description("Draw Method 2 - draw string in rectangle according to Alignment and LineAlignment"+
		             "Calls Method 3")]
		public void Draw(Graphics g, string text, Font font, Brush brush, Rectangle stringRect, StringFormat stringStrFmt)
		{
			int horOffset;
			int vertOffset;

			// Set horizontal offset
			switch (stringStrFmt.Alignment)
			{
				case StringAlignment.Center:
					horOffset = (stringRect.Width / 2) - (int)(font.Size / 2) - 2;
					break;
				case StringAlignment.Far:
					horOffset = (stringRect.Width - (int)font.Size - 2);
					break;
				default:
					horOffset = 0;
					break;
			}

			// Set vertical offset
			
			double textSize = this.Length(text, font);
		
			switch (stringStrFmt.LineAlignment)
			{
				case StringAlignment.Center:
					vertOffset = (stringRect.Height / 2) - (int)(textSize / 2);
					break;
				case StringAlignment.Far:
					vertOffset = stringRect.Height - (int)textSize - 2;
					break;
				default:
					vertOffset = 0;
					break;
			}
			
			// Draw the string using the offsets
			this.Draw(g, text, font, brush, 
			          stringRect.X + horOffset, 
			          stringRect.Y + vertOffset);
		}
		
		[Description("Draw Method 3 - draw string at coordinates x, y")]
		public void Draw(Graphics g, string text, Font font, Brush brush, int x, int y)
		{
			char[] textChars = text.ToCharArray();				// Put the string into array of chars
			StringFormat charStrFmt = new StringFormat();		// Used to align each char centrally
			charStrFmt.Alignment = StringAlignment.Center;
			Rectangle charRect = new Rectangle(x, y, (int)(font.Size * 1.5), font.Height);
			
			for (int i = 0; i < text.Length; i++)
			{
				charRect.Offset(0, ExtraSpaceAllowance(esaType.Pre, textChars[i], font));	// Height allowance
				g.DrawString(textChars[i].ToString(),font,brush, charRect, charStrFmt);		// Write the character
				charRect.Offset(0, (int)(font.Height * textSpread));						// Standard height
				charRect.Offset(0, ExtraSpaceAllowance(esaType.Post, textChars[i],font));	// Height Allowance
			}
		}
		
		[Description("Length Method - returns vertical length of string")]
		public int Length(string text, Font font)
		{
			char[] textChars = text.ToCharArray();		// Put the string into array of chars
			int len = new int();
			
			for (int i = 0; i < text.Length; i++)
			{
				len += (int)(font.Height * textSpread);		// Add height of font, times spread factor.
				len += ExtraSpaceAllowance(esaType.Either, textChars[i], font); // Add allowance
			}
			
			return len;
		}
		
		
		private int ExtraSpaceAllowance(esaType type, char ch, Font font)
		{
			
			if (textSpread >= 1) return 0;				// No action if textSpread 1 or more
			
			int offset = 0;
			
			// Do we need to pad BEFORE the next char?
			if (type == esaType.Pre | type == esaType.Either)
			{
				// Does our character appear in the "pre" list? (ie taller than average)
				if (" bdfhijkltABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".IndexOf(ch) > 0)
				{
					// TODO Allow for textSpread here.
					//offset += (int)(font.Height * .2 * (1 - textSpread));
					offset += (int)(font.Height * .2);
				}
			}
			
			// Do we need to pad AFTER the next char?
			if (type == esaType.Post | type == esaType.Either)
			{
				// Does our character appear in the "post" list? (is dangles over the bottom of the line)
				if (" gjpqyQ".IndexOf(ch) > 0)
				{
					// TODO Allow for textSpread here.
					//offset += (int)(font.Height * .2 * (1 - textSpread));
					offset += (int)(font.Height * .2);
				}
			}
			
			return offset;
		}
	}
}

