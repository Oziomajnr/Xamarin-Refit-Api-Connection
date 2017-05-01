using Android.App;
using Android.OS;
using Android.Widget;
using Refit;
using ConnecingToApiExample.Interface;
using System.Collections.Generic;
using ConnecingToApiExample.Model;
using Android.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System;

namespace ConnecingToApiExample
{
    [Activity(Label = "ConnecingToApiExample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        IGitHubApi gitHubApi;
        List<User> users = new List<User>();
        List<String> user_names = new List<String>();
        Button cake_lyf_button;
        IListAdapter ListAdapter;
        ListView listView;
        


        protected override void OnCreate(Bundle bundle)
        {
           
            try
            {
                base.OnCreate(bundle);

                SetContentView(Resource.Layout.Main);
                cake_lyf_button = FindViewById<Button>(Resource.Id.btn_list_users);
                listView = FindViewById<ListView>(Resource.Id.listview_users);
                cake_lyf_button.Click += Cake_lyf_button_Click;

                JsonConvert.DefaultSettings =()=> new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = { new StringEnumConverter() }
                };

                gitHubApi = RestService.For<IGitHubApi>("https://api.github.com");
                            
            }
            catch (Exception ex)
            {
                Log.Error("Ozioma See", ex.Message);
            }
        }

        private void Cake_lyf_button_Click(object sender, EventArgs e)
        {
            getUsers();

        }

        private async void getUsers()
        {
            try
            {
               ApiResponse response = await gitHubApi.GetUser();
                users = response.items;
                
                foreach (User user in users)
                {
                    user_names.Add(user.ToString());
                }
                ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, user_names);
                listView.Adapter = ListAdapter;

            }
            catch (Exception ex)
            {
                Toast.MakeText(this,ex.StackTrace, ToastLength.Long).Show();
                
            }

        }


    }
}

