﻿using Merlin.Profiles.Gatherer;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Merlin
{
    public class Core
    {
        private const string LogFile = "Logs.txt";

        public static GameObject _coreObject;

        private static Profile _activeProfile;

        public static LineRenderer LineRenderer { get; set; }

        public static void Load()
        {
            _coreObject = new GameObject();
            var gatherer = _coreObject.AddComponent<Gatherer>();
            UnityEngine.Object.DontDestroyOnLoad(_coreObject);
        }

        public static void Unload()
        {
            if (_activeProfile != null)
                _activeProfile.enabled = false;

            _activeProfile = null;

            UnityEngine.Object.Destroy(_coreObject);

            _coreObject = null;
        }

        public static void Log(string message)
        {
            using (var stream = new FileStream(LogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine($"[{DateTime.Now}] {message}");
            }
        }

        public static void Log(Exception e)
        {
            using (var stream = new FileStream(LogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now}: ===================================");
                writer.WriteLine($"{DateTime.Now}: {e.Message}");
                writer.WriteLine($"{DateTime.Now}: {e.StackTrace}");
                writer.WriteLine();
                writer.WriteLine();
            }
        }

        public static void Activate(Profile profile)
        {
            if (_activeProfile != null)
                _activeProfile.enabled = false;

            _activeProfile = profile;
            _activeProfile.enabled = true;
        }

        public static void DeactivateAll()
        {
            var profiles = _coreObject.GetComponents<Profile>();

            foreach (var profile in profiles)
                profile.enabled = false;

            _activeProfile = null;
        }

        private class VersionView : MonoBehaviour
        {
            private Rect _displayRectangle;
            private Version _version;

            private void Start()
            {
                _displayRectangle = new Rect((Screen.width / 2) - 30, 10, 100, 20);
                _version = Assembly.Load("Merlin").GetName().Version;
            }

            private void OnGUI()
            {
                GUI.Label(_displayRectangle, _version.ToString());
            }
        }
    }
}