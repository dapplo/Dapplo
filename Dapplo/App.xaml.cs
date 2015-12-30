/*
 * 
 * Copyright (c) 2016 Dapplo. All rights reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 * MA 02110-1301  USA
 */

using Squirrel;
using System.Windows;

namespace dapplo
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		static bool _showTheWelcomeWizard;

		private async void Application_Startup(object sender, StartupEventArgs e)
		{
			using (var updateManager = new UpdateManager(@"location"))
			{
				// Note, in most of these scenarios, the app exits after this method completes!
				SquirrelAwareApp.HandleEvents(
				  onInitialInstall: v =>
				  {
					  MessageBox.Show("onInitialInstall for: " + updateManager.CurrentlyInstalledVersion().Version);
					  updateManager.CreateShortcutForThisExe();
				  },
				  onAppUpdate: v => {
					  MessageBox.Show("onAppUpdate for: " + updateManager.CurrentlyInstalledVersion().Version);
					  updateManager.CreateShortcutForThisExe();
				  },
				  onAppUninstall: v =>
				  {
					  MessageBox.Show("onAppUninstall for: " + updateManager.CurrentlyInstalledVersion().Version);
					  updateManager.RemoveShortcutForThisExe();
				  },
				  onAppObsoleted: v =>
				  {
					  MessageBox.Show("onAppObsoleted: so long and thanks for all the fish from: " + updateManager.CurrentlyInstalledVersion().Version);
				  },
				  onFirstRun: () => {
					  MessageBox.Show($"onFirstRun for: {updateManager.CurrentlyInstalledVersion().Version}");
					  _showTheWelcomeWizard = true;
				  });


				if (_showTheWelcomeWizard)
				{
					MessageBox.Show("Hello world, I am here for the first time!");
				}
				MessageBox.Show($"I am {updateManager.ApplicationName} version = {updateManager.CurrentlyInstalledVersion().Version}");

				var releaseEntry = await updateManager.UpdateApp();
				if (releaseEntry != null && releaseEntry.Version != null)
				{
					MessageBox.Show($"Updated to version: {releaseEntry.Version.Version} which will run at next start.");
				}
			}

			// do something
			Shutdown();
		}
	}
}
