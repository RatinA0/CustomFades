using System;
using System.Collections.Generic;
using System.Windows.Forms;

// Sony.Vegas if your version below is 13
// Project -> Add reference -> Browse -> VEGAS install folder -> ScriptPortal.Vegas.dll
using ScriptPortal.Vegas;
//using Sony.Vegas;

namespace CustomFades
{
	public partial class wndForm : Form
	{
		// set global variables so we can set stuff for vegas
		public int FadeInValue;
		public int FadeOutValue;
		public string FadeInTimecode;
		public string FadeOutTimecode;
		public string FadeInCurve;
		public string FadeOutCurve;

		// Change Fade In/Out Curve
		public bool changeFIC = false;
		public bool changeFOC = false;

		// Fade In/Out To Zero
		public bool FITZ = false;
		public bool FOTZ = false;

		// Add (to) Fade In/Out Length
		public bool AFIL = false;
		public bool AFOL = false;

		// Reduce (to) Fade In/Out Length
		public bool RFIL = false;
		public bool RFOL = false;

		public wndForm()
		{
			InitializeComponent();
			FormBorderStyle = FormBorderStyle.FixedSingle; 
			nudFadeIn.Select();
		}

		private void btRun_Click(object sender, EventArgs e)
		{
			FadeInValue = (int)nudFadeIn.Value;
			FadeOutValue = (int)nudFadeOut.Value;
			FadeInTimecode = cbFadeInTimecode.Text;
			FadeOutTimecode = cbFadeOutTimecode.Text;

			if (changeFIC)
			{
				FadeInCurve = cbFadeInCurve.Text;
			}
			if (changeFOC)
			{
				FadeOutCurve = cbFadeOutCurve.Text;
			}

			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Will change the curve type too if the checkbox is checked
		/// </summary>
		private void ChangeCurveType(CheckBox checkBox ,bool changeCurveBool, ComboBox comboBox)
		{
			if (checkBox.Checked)
			{
				changeCurveBool = true;
				comboBox.Enabled = true;
			}
			else
			{
				changeCurveBool = false;
				comboBox.Enabled = false;
			}
		}
		private void cbxChangeFadeInCurveType_CheckedChanged(object sender, EventArgs e)
		{
			ChangeCurveType(cbxChangeFadeInCurveType, changeFIC, cbFadeInCurve);
		}
		private void cbxChangeFadeOutCurveType_CheckedChanged(object sender, EventArgs e)
		{
			ChangeCurveType(cbxChangeFadeOutCurveType, changeFOC, cbFadeOutCurve);
		}

		/// <summary>
		/// Will change the fade length to 0 if the checkbox is checked
		/// </summary>
		private void FadeToZero(CheckBox checkBox, bool fadeBool, NumericUpDown numericUpDown, ComboBox comboBox, CheckBox addCheckBox, CheckBox reduceCheckBox)
		{
			if (checkBox.Checked)
			{
				fadeBool = true;
				numericUpDown.Enabled = false;
				comboBox.Enabled = false;
				addCheckBox.Enabled = false;
				reduceCheckBox.Enabled = false;
			}
			else
			{
				fadeBool = false;
				numericUpDown.Enabled = true;
				comboBox.Enabled = true;
				addCheckBox.Enabled = true;
				reduceCheckBox.Enabled = true;
			}
		}
		private void cbxFadeInToZero_CheckedChanged(object sender, EventArgs e)
		{
			FadeToZero(cbxFadeInToZero, FITZ, nudFadeIn, cbFadeInTimecode, cbxFIAddLength, cbxFIReduceLength);
		}
		private void cbxFadeOutToZero_CheckedChanged(object sender, EventArgs e)
		{
			FadeToZero(cbxFadeOutToZero, FOTZ, nudFadeOut, cbFadeOutTimecode, cbxFOAddLength, cbxFOReduceLength);
		}

		/// <summary>
		/// Clicking check box disables the other one and setting the text of the correct labels
		/// </summary>
		private void SetLength(Label fadeLabel, string labelText, CheckBox checkBox_checked, bool fadeBool, CheckBox checkBox_other, CheckBox checkBox_setZero)
		{
			fadeLabel.Text = "New Length:";

			if (checkBox_checked.Checked)
			{
				fadeLabel.Text = labelText;
				fadeBool = true;
				checkBox_other.Enabled = false;
				checkBox_setZero.Enabled = false;
			}
			else
			{
				fadeBool = false;
				checkBox_other.Enabled = true;
				checkBox_setZero.Enabled = true;
			}
		}

		/// <summary>
		/// Option to add the current length of the Fades instead of setting it
		/// </summary>
		private void cbxFIAddLength_CheckedChanged(object sender, EventArgs e)
		{
			SetLength(lbFadeIn, "Increase by:", cbxFIAddLength, AFIL, cbxFIReduceLength, cbxFadeInToZero);
		}
		private void cbxFOAddLength_CheckedChanged(object sender, EventArgs e)
		{
			SetLength(lbFadeOut, "Increase by:", cbxFOAddLength, AFOL, cbxFOReduceLength, cbxFadeOutToZero);
		}

		/// <summary>
		/// Option to reduce the current length of the Fades instead of setting it
		/// </summary>
		private void cbxFIReduceLength_CheckedChanged(object sender, EventArgs e)
		{
			SetLength(lbFadeIn, "Reduce by:", cbxFIReduceLength, RFIL, cbxFIAddLength, cbxFadeInToZero);
		}
		private void cbxFOReduceLength_CheckedChanged(object sender, EventArgs e)
		{
			SetLength(lbFadeOut, "Reduce by:", cbxFOReduceLength, RFOL, cbxFOAddLength, cbxFadeOutToZero);
		}

		/// <summary>
		/// Clicking the Help button
		/// </summary>
		private void btnHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("- - - - - - - - - - - What can you do? - - - - - - - - - - -"		+ "\n\n" +
							"You can set the Fade In/Out:"										+ "\n\n" +
							"  - Length (in either Frames or Seconds)"							+ "\n" +
							"    > If Length stays 0 -> it stays the same"						+ "\n" +
							"    > Use the CheckBox if you want the Fade In/Out length to be 0"	+ "\n\n" +
							"  - Curve Type (Fast, Linear, Sharp, Slow, Smooth)"				+ "\n" +
							"    > It doesn't change the Curve Type by default"					+ "\n" +
							"    > Use the CheckBox if you want to change it", "Help", MessageBoxButtons.OK);
		}
	}

	// Vegas pro gets this
	public class EntryPoint
	{
		public void FromVegas(Vegas myVegas)
		{
			// setting local variables for later
			int FadeInValue;
			int FadeOutValue;
			string FadeInTimecode;
			string FadeOutTimecode;
			bool changeFIC;
			bool changeFOC;
			string FadeInCurve;
			string FadeOutCurve;
			bool FITZ;
			bool FOTZ;
			bool AFIL;
			bool AFOL;
			bool RFIL;
			bool RFOL;

			using (wndForm form = new wndForm())
			{
				DialogResult result = form.ShowDialog();
				if (result == DialogResult.OK)
				{
					// get values from the form to work with
					FadeInValue = form.FadeInValue;
					FadeOutValue = form.FadeOutValue;
					FadeInTimecode = form.FadeInTimecode;
					FadeOutTimecode = form.FadeOutTimecode;
					changeFIC = form.changeFIC;
					changeFOC = form.changeFOC;
					FITZ = form.FITZ;
					FOTZ = form.FOTZ;
					AFIL = form.AFIL;
					AFOL = form.AFOL;
					RFIL = form.RFIL;
					RFOL = form.RFOL;
					FadeInCurve = form.FadeInCurve;
					FadeOutCurve = form.FadeOutCurve;
				}
				else
				{
					return;
				}
			}

			// try catch for safety
			try
			{
				TrackEvent[] selectedEvents = GetSelectedEvents(myVegas.Project);

				// for every selected event
				foreach (TrackEvent trackEvent in selectedEvents)
				{
					FrameChecker(FadeInValue, FadeInTimecode, RFIL, AFIL, FITZ, trackEvent);
					FrameChecker(FadeOutValue, FadeOutTimecode, RFOL, AFOL, FOTZ, trackEvent);

					ChangeCurveTypes(changeFIC, FadeInCurve, trackEvent);
					ChangeCurveTypes(changeFOC, FadeOutCurve, trackEvent);
				}
			}
			catch (Exception ex)
			{
				// show error message if any
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Calculating for frames or seconds
		/// </summary>
		private void FrameChecker(int fadeValue, string fadeTimecode, bool reduceBool, bool addBool, bool fadeToZeroBool, TrackEvent trackEvent)
		{
			// if fade value is NOT 0 -> change stuff
			if (fadeValue > 0)
			{
				switch (fadeTimecode)
				{
					// when we want to count in frames
					case "Frames":
						FrameCalculator(addBool, reduceBool, trackEvent, Timecode.FromFrames(fadeValue));
						break;
					// when we want to count in seconds
					case "Seconds":
						FrameCalculator(addBool, reduceBool, trackEvent, Timecode.FromSeconds(fadeValue));
						break;
					// save a heartbeat if the box is empty
					default:
						FrameCalculator(addBool, reduceBool, trackEvent, Timecode.FromFrames(fadeValue));
						break;
				}
			}
			// if fade to zero is checked, it's set length to 0
			else if (fadeToZeroBool && fadeValue == 0)
			{
				trackEvent.FadeIn.Length = Timecode.FromFrames(0);
			}
		}
		private void FrameCalculator(bool addBool, bool reduceBool, TrackEvent trackEvent, Timecode timecode)
        {
			if (reduceBool)
			{
				trackEvent.FadeIn.Length = trackEvent.FadeIn.Length - timecode;
			}
			else if (addBool)
			{
				trackEvent.FadeIn.Length = trackEvent.FadeIn.Length + timecode;
			}
			else
			{
				trackEvent.FadeIn.Length = timecode;
			}
		}

		/// <summary>
		/// Change curve types
		/// </summary>
		private void ChangeCurveTypes(bool fadeCurve, string fadeString, TrackEvent trackEvent)
        {
			// if changing curve type is enabled for fade in
			if (fadeCurve)
			{
				// change fade in curve type according to the selection
				switch (fadeString)
				{
					case "Fast":
						trackEvent.FadeIn.Curve = CurveType.Fast;
						break;
					case "Linear":
						trackEvent.FadeIn.Curve = CurveType.Linear;
						break;
					case "Sharp":
						trackEvent.FadeIn.Curve = CurveType.Sharp;
						break;
					case "Slow":
						trackEvent.FadeIn.Curve = CurveType.Slow;
						break;
					case "Smooth":
						trackEvent.FadeIn.Curve = CurveType.Smooth;
						break;
					// if the box is empty
					default:
						trackEvent.FadeIn.Curve = CurveType.Smooth;
						break;
				}
			}
		}

		// getting selected items into a list, from layer 1 left to right to last layer a -> z
		TrackEvent[] GetSelectedEvents(Project project)
		{
			List<TrackEvent> selectedList = new List<TrackEvent>();
			foreach (Track track in project.Tracks)
			{
				foreach (TrackEvent trackEvent in track.Events)
				{
					if (trackEvent.Selected)
					{
						selectedList.Add(trackEvent);
					}
				}
			}
			return selectedList.ToArray();
		}
	}
}
