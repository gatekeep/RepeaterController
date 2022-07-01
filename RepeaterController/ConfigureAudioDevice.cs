/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using NAudio;
using NAudio.Wave;

using RepeaterController.Xml;

namespace RepeaterController
{
    /// <summary>
    /// Audio Configuration Modal
    /// </summary>
    public partial class ConfigureAudioDevice : Form
    {
        /**
         * Fields
         */
        private RepeaterOptions options;
        private XmlResource rsrc;

        /// <summary>
        /// Index of the primary local input audio device.
        /// </summary>
        [XmlImportExport]
        public int WaveInDevice;
        /// <summary>
        /// Index of the primary link input audio device.
        /// </summary>
        [XmlImportExport]
        public int WaveLinkInDevice;
        /// <summary>
        /// Index of the primary output audio device.
        /// </summary>
        [XmlImportExport]
        public int WaveOutDevice;

        /// <summary>
        /// Number of milliseconds to buffer audio for.
        /// </summary>
        [XmlImportExport]
        public int BufferMilliseconds;
        /// <summary>
        /// Number of milliseconds to buffer MDC audio for.
        /// </summary>
        [XmlImportExport]
        public int AFSKBufferMilliseconds;

        /// <summary>
        /// Flag indicating whether we are using separate link audio path.
        /// </summary>
        [XmlImportExport]
        public bool SeparateLinkAudio;

        /// <summary>
        /// Flag indicating whether we are disabling audio filtering.
        /// </summary>
        [XmlImportExport]
        public bool DisableFiltering;

        /// <summary>
        /// Number of audio buffers.
        /// </summary>
        [XmlImportExport]
        public int BufferCount;
        /// <summary>
        /// Number of MDC audio buffers.
        /// </summary>
        [XmlImportExport]
        public int AFSKBufferCount;

        /**
         * Class
         */
        private class ListItem
        {
            /**
             * Fields
             */
            public string Description;
            public int Index;

            /**
             * Methods
             */
            /// <summary>
            /// Initializes a new instance of the <see cref="ListItem"/> class.
            /// </summary>
            public ListItem(int idx, string desc)
            {
                this.Description = desc;
                this.Index = idx;
            }

            public override string ToString()
            {
                return Description;
            }
        } // private class ListItem

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureAudioDevice"/> class.
        /// </summary>
        /// <param name="options"></param>
        public ConfigureAudioDevice(RepeaterOptions options)
        {
            InitializeComponent();

            this.options = options;

            SetDefaults();
            UpdateModal();

            this.FormClosing += ConfigureAudioDevice_FormClosing;

            checkBoxSeparateLinkAudio.CheckedChanged += CheckBoxSeparateLinkAudio_CheckedChanged;
            checkBoxDisableFilter.CheckedChanged += CheckBoxDisableFilter_CheckedChanged;

            bufferLengthTextBox.TextChanged += BufferTextBox_TextChanged;
            dataBufferLengthTextBox.TextChanged += DataBufferTextBox_TextChanged;

            bufferCountTextBox.TextChanged += BufferCountTextBox_TextChanged;
            dataBufferCountTextBox.TextChanged += DataBufferCountTextBox_TextChanged;
#if MDC_CONSOLE
            inputDeviceComboBox.Enabled = false;

            bufferLengthTextBox.Enabled = false;
            bufferCountTextBox.Enabled = false;

            bufferDetailLabel.Visible = false;
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureAudioDevice"/> class.
        /// </summary>
        /// <param name="rsrc"></param>
        /// <param name="options"></param>
        public ConfigureAudioDevice(XmlResource rsrc, RepeaterOptions options) : this(options)
        {
            this.rsrc = rsrc;

            LoadXml();
            UpdateModal();
        }

        /// <summary>
        /// Internal helper to load/generate the user XML configuration data.
        /// </summary>
        private void LoadXml()
        {
            if (rsrc.HasNode("AudioSettings"))
            {
                // get data from XML
                try
                {
                    XmlResource audioSettings = rsrc.Get<XmlResource>("AudioSettings");
                    audioSettings.GetByReflection(this);
                }
                catch (ArgumentException ae)
                {
                    // this will occur when a value isn't found
                    Messages.TraceException(ae.Message, ae);
                }
            }
            else
            {
                // reset defaults
                SetDefaults();
                SaveXml();
            }
            UpdateModal();
        }

        /// <summary>
        /// Internal helper to save the XML.
        /// </summary>
        private void SaveXml()
        {
            XmlResource audioSettings = rsrc.CreateNode("AudioSettings");
            audioSettings.SubmitByReflection(this);

            rsrc.SaveXml(Environment.CurrentDirectory + Path.DirectorySeparatorChar + MainWindow.XML_FILE);
        }

        /// <summary>
        /// Sets default preset values.
        /// </summary>
        public void SetDefaults()
        {
            this.WaveInDevice = 0;
            this.WaveLinkInDevice = 0;
            this.WaveOutDevice = 0;
            this.SeparateLinkAudio = false;
            this.DisableFiltering = false;
            this.BufferMilliseconds = 75;
            this.AFSKBufferMilliseconds = 500;
            this.BufferCount = 3;
            this.AFSKBufferCount = 6;
        }

        /// <summary>
        /// Updates the modal to the state of the internal variables.
        /// </summary>
        public void UpdateModal()
        {
            int waveInDevices = WaveIn.DeviceCount;
            inputDeviceComboBox.Items.Clear();
            linkDeviceComboBox.Items.Clear();
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                try
                {
                    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                    inputDeviceComboBox.Items.Add(new ListItem(waveInDevice, deviceInfo.ProductName));
                    linkDeviceComboBox.Items.Add(new ListItem(waveInDevice, deviceInfo.ProductName));
                }
                catch (MmException)
                {
                    inputDeviceComboBox.Items.Add(new ListItem(waveInDevice, "Unknown [" + waveInDevice + "]"));
                    linkDeviceComboBox.Items.Add(new ListItem(waveInDevice, "Unknown [" + waveInDevice + "]"));

                }
            }

            int waveOutDevices = WaveOut.DeviceCount;
            outputDeviceComboBox.Items.Clear();
            for (int waveOutDevice = 0; waveOutDevice < waveOutDevices; waveOutDevice++)
            {
                try
                {
                    WaveOutCapabilities deviceInfo = WaveOut.GetCapabilities(waveOutDevice);
                    outputDeviceComboBox.Items.Add(new ListItem(waveOutDevice, deviceInfo.ProductName));
                }
                catch (MmException)
                {
                    outputDeviceComboBox.Items.Add(new ListItem(waveOutDevice, "Unknown [" + waveOutDevice + "]"));
                }
            }

            if (options.Options.MDCConsoleOnly)
            {
                checkBoxSeparateLinkAudio.Enabled = false;
                SeparateLinkAudio = false;
            }
            else
                checkBoxSeparateLinkAudio.Enabled = true;

            inputDeviceComboBox.SelectedIndex = this.WaveInDevice;
            linkDeviceComboBox.SelectedIndex = this.WaveLinkInDevice;
            outputDeviceComboBox.SelectedIndex = this.WaveOutDevice;
            checkBoxSeparateLinkAudio.Checked = this.SeparateLinkAudio;
            if (SeparateLinkAudio)
                linkDeviceComboBox.Enabled = true;
            else
                linkDeviceComboBox.Enabled = false;

            checkBoxDisableFilter.Checked = this.DisableFiltering;

            bufferLengthTextBox.Text = this.BufferMilliseconds.ToString();
            dataBufferLengthTextBox.Text = this.AFSKBufferMilliseconds.ToString();
            bufferCountTextBox.Text = this.BufferCount.ToString();
            dataBufferCountTextBox.Text = this.AFSKBufferCount.ToString();

            bufferDetailLabel.Text = this.BufferCount + " buffers x " + this.BufferMilliseconds + " ms/buffer = " + this.BufferCount * this.BufferMilliseconds + " ms/captured audio";
            mdcBufferDetail.Text = this.AFSKBufferCount + " buffers x " + this.AFSKBufferMilliseconds + " ms/buffer = " + this.AFSKBufferCount * this.AFSKBufferMilliseconds + " ms/captured audio";
        }

        /// <summary>
        /// Occurs when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigureAudioDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            okButton_Click(sender, new EventArgs());
        }

        /// <summary>
        /// Event that occurs when the "Disable Filtering" checkbox is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxDisableFilter_CheckedChanged(object sender, EventArgs e)
        {
            this.DisableFiltering = checkBoxDisableFilter.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Event that occurs when the "Separate Link Audio" checkbox is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxSeparateLinkAudio_CheckedChanged(object sender, EventArgs e)
        {
            this.SeparateLinkAudio = checkBoxSeparateLinkAudio.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Event that occurs when the MDC buffer textbox changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBufferTextBox_TextChanged(object sender, EventArgs e)
        {
            int defaultValue = this.AFSKBufferMilliseconds;
            int buffer = defaultValue;
            if (int.TryParse(dataBufferLengthTextBox.Text, out buffer))
            {
                dataBufferLengthTextBox.ForeColor = Color.Black;
                this.AFSKBufferMilliseconds = buffer;
            }
            else
            {
                dataBufferLengthTextBox.ForeColor = Color.Red;
                this.AFSKBufferMilliseconds = defaultValue;
            }
            UpdateModal();
        }

        /// <summary>
        /// Event that occurs when the buffer textbox changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BufferTextBox_TextChanged(object sender, EventArgs e)
        {
            int defaultValue = BufferMilliseconds;
            int buffer = defaultValue;
            if (int.TryParse(bufferLengthTextBox.Text, out buffer))
            {
                bufferLengthTextBox.ForeColor = Color.Black;
                this.BufferMilliseconds = buffer;
            }
            else
            {
                bufferLengthTextBox.ForeColor = Color.Red;
                this.BufferMilliseconds = defaultValue;
            }
            UpdateModal();
        }

        /// <summary>
        /// Event that occurs when the MDC buffer count textbox changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBufferCountTextBox_TextChanged(object sender, EventArgs e)
        {
            int defaultValue = AFSKBufferCount;
            int count = defaultValue;
            if (int.TryParse(dataBufferCountTextBox.Text, out count))
            {
                dataBufferCountTextBox.ForeColor = Color.Black;
                this.AFSKBufferCount = count;
            }
            else
            {
                dataBufferCountTextBox.ForeColor = Color.Red;
                this.AFSKBufferCount = defaultValue;
            }
            UpdateModal();
        }

        /// <summary>
        /// Event that occurs when the buffer count textbox changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BufferCountTextBox_TextChanged(object sender, EventArgs e)
        {
            int defaultValue = BufferCount;
            int count = defaultValue;
            if (int.TryParse(bufferCountTextBox.Text, out count))
            {
                bufferCountTextBox.ForeColor = Color.Black;
                this.BufferCount = count;
            }
            else
            {
                bufferCountTextBox.ForeColor = Color.Red;
                this.BufferCount = defaultValue;
            }
            UpdateModal();
        }

        /// <summary>
        /// Event that occurs when the "Ok" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.WaveInDevice = ((ListItem)inputDeviceComboBox.SelectedItem).Index;
            this.WaveLinkInDevice = ((ListItem)linkDeviceComboBox.SelectedItem).Index;
            this.WaveOutDevice = ((ListItem)outputDeviceComboBox.SelectedItem).Index;
            DialogResult = DialogResult.OK;
            SaveXml();

            this.Hide();
        }
    } // public partial class ConfigureAudioDevice : Form
} // namespace RepeaterController
