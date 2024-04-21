using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

namespace FlawareStudios.Translation
{
    public class TranslationEditorWindow : EditorWindow
    {
        public enum TranslationType
        {
            All,
            Text,
            Sprite,
            Audio
        }

        private TranslationModule translationModule;

        private TranslationType currentFilterType;
        private string searchText;
        private int displayCap = 20;
        private int displayPage;

        private int languageCount;

        private Vector2 scrollPosition;

        private bool filterSettingsToggle;
        private bool errorReturnsToggle;
        private bool toolsToggle;

        #region Styles

        private Vector2 minEditorSize = new Vector2(350, 500);

        //Base styles
        private GUIStyle headerTextStyle;
        private GUIStyle textStyle;
        private GUIStyle clickableTextStyle;
        private GUIStyle editableTextStyle;
        private GUIStyle nonEditableTextStyle;

        private const float OBJECT_FIELD_WIDTH = 280;
        private const float OBJECT_FIELD_HEIGHT = 30;

        private const float ENUM_FIELD_WIDTH = 75;

        private const float SHOWN_TRANSLATION_HEAD_HEIGHT = 40;

        private const float SHOWN_TRANSLATION_KEY_MIN_WIDTH = 75;
        private const float SHOWN_TRANSLATION_KEY_MAX_WIDTH = 150;
        private const float SHOWN_TRANSLATION_MIN_WIDTH = 100;
        private const float SHOWN_TRANSLATION_MAX_WIDTH = 250;

        private const float TOOLS_SECTION_MAX_HEIGHT = 0;

        GUILayoutOption[] listItemKeyLayoutOptions;
        GUILayoutOption[] listItemLayoutOptions;

        //Group styles
        private GUIStyle headerStyle;
        private GUIStyle mainStyle;
        private GUIStyle toolsStyle;
        private GUIStyle listStyle;

        private GUIStyle listItemStyle;

        #endregion

        private void OnEnable()
        {
            LoadStyles();
        }

        private void LoadStyles()
        {
            headerTextStyle = new GUIStyle()
            {
                fontSize = 25,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,

                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                }
            };

            textStyle = new GUIStyle()
            {
                fontSize = 12,

                wordWrap = true,

                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                }
            };

            clickableTextStyle = new GUIStyle(textStyle)
            {
                stretchHeight = true,

                fontStyle = FontStyle.Bold,

                alignment = TextAnchor.MiddleLeft,

                padding = new RectOffset(3, 3, 3, 3),
            };

            editableTextStyle = new GUIStyle(textStyle)
            {
                stretchHeight = true,

                alignment = TextAnchor.UpperLeft,

                margin = new RectOffset(5, 5, 0, 0),
                padding = new RectOffset(8, 8, 8, 8),
            };
            editableTextStyle.normal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f));

            nonEditableTextStyle = new GUIStyle(editableTextStyle)
            {
                fontStyle = FontStyle.Bold,

                normal = textStyle.normal,
            };

            headerStyle = new GUIStyle()
            {
                stretchWidth = true,

                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(10, 10, 15, 15),

                normal = new GUIStyleState()
                {
                    background = MakeTex(1, 1, new Color(0.12f, 0.12f, 0.12f)),
                }
            };

            mainStyle = new GUIStyle()
            {
                stretchWidth = true,
                stretchHeight = true,

                padding = new RectOffset(10, 10, 10, 10),

                normal = new GUIStyleState()
                {
                    background = MakeTex(1, 1, new Color(0.15f, 0.15f, 0.15f)),
                }
            };

            toolsStyle = new GUIStyle()
            {
                stretchWidth = true,
                stretchHeight = false,
            };

            listStyle = new GUIStyle()
            {
                stretchWidth = true,
                stretchHeight = true,
            };

            listItemStyle = new GUIStyle()
            {
                stretchHeight = true,

                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 3, 3),
                padding = new RectOffset(2, 2, 4, 4),

                normal = new GUIStyleState()
                {
                    background = MakeTex(1, 1, new Color(0.12f, 0.12f, 0.12f)),
                },
            };

            listItemKeyLayoutOptions = new GUILayoutOption[]
            {
                GUILayout.MinWidth(SHOWN_TRANSLATION_KEY_MIN_WIDTH),
                GUILayout.MaxWidth(SHOWN_TRANSLATION_KEY_MAX_WIDTH),
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true),
            };

            listItemLayoutOptions = new GUILayoutOption[]
            {
                GUILayout.MinWidth(SHOWN_TRANSLATION_MIN_WIDTH),
                GUILayout.MaxWidth(SHOWN_TRANSLATION_MAX_WIDTH),
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true),
            };
        }

        [MenuItem("Tools/Translation Editor")]
        public static void OpenEditorWidnow()
        {
            TranslationEditorWindow window = CreateWindow<TranslationEditorWindow>("Translation Editor Window");

            window.minSize = window.minEditorSize;
        }

        public static void OpenEditorWindow(TranslationModule translationModule)
        {
            TranslationEditorWindow window = CreateWindow<TranslationEditorWindow>("Translation Editor Window");
            window.translationModule = translationModule;

            window.minSize = window.minEditorSize;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(headerStyle);
            EditorGUILayout.LabelField("Translation Editor", headerTextStyle);

            EditorGUILayout.Space(10);

            DrawTranslationModuleSelection();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(mainStyle);
            if (translationModule != null)
            {
                EditorGUILayout.BeginVertical(GUILayout.MaxHeight(TOOLS_SECTION_MAX_HEIGHT));

                if (GUILayout.Button("Filter Settings", clickableTextStyle)) filterSettingsToggle = !filterSettingsToggle;
                if (filterSettingsToggle)
                    DrawFilterOptions();

                if (GUILayout.Button("Error Returns", clickableTextStyle)) errorReturnsToggle = !errorReturnsToggle;
                if (errorReturnsToggle)
                    DrawErrorReturns();

                if (GUILayout.Button("Tools", clickableTextStyle)) toolsToggle = !toolsToggle;
                if (toolsToggle)
                    DrawToolkit();

                DrawPages();

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5);

                EditorGUILayout.BeginVertical(listStyle, GUILayout.ExpandHeight(false));

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                DrawTranslationList();
                EditorGUILayout.EndScrollView();

                GUILayout.FlexibleSpace();

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            if (translationModule != null)
                EditorUtility.SetDirty(translationModule);
        }

        private void DrawFilterOptions()
        {
            EditorGUILayout.BeginHorizontal();
            searchText = EditorGUILayout.TextField(searchText, EditorStyles.toolbarSearchField);
            currentFilterType = (TranslationType)EditorGUILayout.EnumPopup(currentFilterType, GUILayout.Width(ENUM_FIELD_WIDTH));
            EditorGUILayout.EndHorizontal();

            displayCap = Mathf.Max(EditorGUILayout.IntField("Display Cap", displayCap), 1);
        }

        private void DrawToolkit()
        {
            EditorGUILayout.BeginHorizontal();
            languageCount = EditorGUILayout.IntField(languageCount);

            GUI.enabled = languageCount != translationModule.translatedLanguages.Length;
            if (GUILayout.Button("Set Language Count"))
            {
                translationModule.SetLanguageCount(languageCount);
                GUIUtility.keyboardControl = 0;
            }
            GUI.enabled = true;

            if (GUILayout.Button("Optimize Languages"))
            {
                translationModule.SetLanguageCount(translationModule.translatedLanguages.Length, true);

                GUIUtility.keyboardControl = 0;
            }

            if (GUILayout.Button("Delete Empty Keys"))
            {
                translationModule.DeleteEmptyKeys();

                GUIUtility.keyboardControl = 0;
            }


            if (GUILayout.Button("Sort"))
            {
                List<TranslationCell<string>> textTranslations = new List<TranslationCell<string>>(translationModule.textTranslations);
                textTranslations.Sort((a, b) => a.key.CompareTo(b.key));
                translationModule.textTranslations = textTranslations.ToArray();

                List<TranslationCell<Sprite>> spriteTranslations = new List<TranslationCell<Sprite>>(translationModule.spriteTranslations);
                spriteTranslations.Sort((a, b) => a.key.CompareTo(b.key));
                translationModule.spriteTranslations = spriteTranslations.ToArray();

                List<TranslationCell<AudioClip>> audioTranslations = new List<TranslationCell<AudioClip>>(translationModule.audioTranslations);
                audioTranslations.Sort((a, b) => a.key.CompareTo(b.key));
                translationModule.audioTranslations = audioTranslations.ToArray();

                GUIUtility.keyboardControl = 0;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add TextT")) { translationModule.AddEmptyTranslation(ref translationModule.textTranslations); GUIUtility.keyboardControl = 0; }
            if (GUILayout.Button("Add SpriteT")) { translationModule.AddEmptyTranslation(ref translationModule.spriteTranslations); GUIUtility.keyboardControl = 0; }
            if (GUILayout.Button("Add AudioT")) { translationModule.AddEmptyTranslation(ref translationModule.audioTranslations); GUIUtility.keyboardControl = 0; }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawErrorReturns()
        {
            EditorGUILayout.BeginHorizontal();
            translationModule.errorText = EditorGUILayout.TextArea(translationModule.errorText, editableTextStyle, GUILayout.MinHeight(30), GUILayout.ExpandWidth(true));
            translationModule.errorSprite = (Sprite)EditorGUILayout.ObjectField(translationModule.errorSprite, typeof(Sprite), false, GUILayout.MinHeight(30), GUILayout.ExpandWidth(true));
            translationModule.errorAudio = (AudioClip)EditorGUILayout.ObjectField(translationModule.errorAudio, typeof(AudioClip), false, GUILayout.MinHeight(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
        }

        private int GetRowsToDisplay()
        {
            int returned = 0;
            if(currentFilterType == TranslationType.All || currentFilterType == TranslationType.Text) 
                foreach (var item in translationModule.textTranslations)
                {
                    if (!string.IsNullOrWhiteSpace(searchText) && !item.key.StartsWith(searchText, System.StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    returned++;
                }
            if (currentFilterType == TranslationType.All || currentFilterType == TranslationType.Sprite)
                foreach (var item in translationModule.spriteTranslations)
                {
                    if (!string.IsNullOrWhiteSpace(searchText) && !item.key.StartsWith(searchText, System.StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    returned++;
                }
            if (currentFilterType == TranslationType.All || currentFilterType == TranslationType.Audio)
                foreach (var item in translationModule.audioTranslations)
                {
                    if (!string.IsNullOrWhiteSpace(searchText) && !item.key.StartsWith(searchText, System.StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    returned++;
                }

            return returned;
        }

        private void DrawPages()
        {
            int rowsToDisplay = GetRowsToDisplay();
            int pages = rowsToDisplay / displayCap;

            Debug.Log(rowsToDisplay);

            if (rowsToDisplay % displayCap != 0)
                pages++;

            if (pages > 0 && displayPage >= pages - 1)
                displayPage = pages - 1;

            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < pages; i++)
            {
                GUI.enabled = i != displayPage;
                if(GUILayout.Button($"{i + 1}", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    int temp = i;
                    displayPage = temp;

                    scrollPosition = new Vector2();
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTranslationList()
        {
            int translatedLanguages = translationModule.translatedLanguages.Length;

            EditorGUILayout.BeginHorizontal(listItemStyle, GUILayout.MinHeight(SHOWN_TRANSLATION_HEAD_HEIGHT));
            GUI.enabled = false;
            EditorGUILayout.TextField("Key", nonEditableTextStyle, listItemKeyLayoutOptions);
            GUI.enabled = true;
            for (int i = 0; i < translatedLanguages; i++)
            {
                translationModule.translatedLanguages[i] = EditorGUILayout.TextField(translationModule.translatedLanguages[i], editableTextStyle, listItemLayoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            int displayedTranslations = 0;

            switch (currentFilterType)
            {
                case TranslationType.All:
                    DrawTextTranslationList(translatedLanguages, ref displayedTranslations);
                    DrawSpriteTranslationList(translatedLanguages, ref displayedTranslations);
                    DrawAudioTranslationList(translatedLanguages, ref displayedTranslations);
                    break;

                case TranslationType.Text:
                    DrawTextTranslationList(translatedLanguages, ref displayedTranslations);
                    break;

                case TranslationType.Sprite:
                    DrawSpriteTranslationList(translatedLanguages, ref displayedTranslations);
                    break;

                case TranslationType.Audio:
                    DrawAudioTranslationList(translatedLanguages, ref displayedTranslations);
                    break;
            }
        }

        private bool DrawTextTranslationList(int translatedLanguages, ref int displayedTranslations)
        {
            if (displayedTranslations >= displayCap)
                return true;

            List<TranslationCell<string>> translations = new(translationModule.textTranslations);
            if (!string.IsNullOrWhiteSpace(searchText))
                translations = translations.Where(x => x.key.StartsWith(searchText, System.StringComparison.InvariantCultureIgnoreCase)).ToList();

            for (int i = displayPage * displayCap; i < translations.Count; i++)
            {
                TranslationCell<string> temp = translations[i];

                displayedTranslations++;

                EditorGUILayout.BeginHorizontal(listItemStyle);

                temp.key = EditorGUILayout.TextArea(temp.key, editableTextStyle, listItemKeyLayoutOptions);

                for (int j = 0; j < translatedLanguages; j++)
                {
                    temp.translations[j] = EditorGUILayout.TextArea(temp.translations[j], editableTextStyle, listItemLayoutOptions);
                }

                EditorGUILayout.EndHorizontal();

                if (displayedTranslations >= displayCap)
                    return true;
            }

            return false;
        }

        private bool DrawSpriteTranslationList(int translatedLanguages, ref int displayedTranslations)
        {
            if (displayedTranslations >= displayCap)
                return true;

            List<TranslationCell<Sprite>> translations = new (translationModule.spriteTranslations);
            if(!string.IsNullOrWhiteSpace(searchText))
                translations = translations.Where(x => x.key.StartsWith(searchText, System.StringComparison.InvariantCultureIgnoreCase)).ToList();

            for (int i = displayPage * displayCap; i < translations.Count; i++)
            {
                TranslationCell<Sprite> temp = translations[i];

                displayedTranslations++;

                EditorGUILayout.BeginHorizontal(listItemStyle);

                temp.key = EditorGUILayout.TextArea(temp.key, editableTextStyle, listItemKeyLayoutOptions);

                for (int j = 0; j < translatedLanguages; j++)
                {
                    temp.translations[j] = (Sprite)EditorGUILayout.ObjectField(temp.translations[j], typeof(Sprite), false, listItemLayoutOptions);
                }

                EditorGUILayout.EndHorizontal();

                if (displayedTranslations >= displayCap)
                    return true;
            }

            return false;
        }

        private bool DrawAudioTranslationList(int translatedLanguages, ref int displayedTranslations)
        {
            if (displayedTranslations >= displayCap)
                return true;

            List<TranslationCell<AudioClip>> translations = new (translationModule.audioTranslations);
            if (!string.IsNullOrWhiteSpace(searchText))
                translations = translations.Where(x => x.key.StartsWith(searchText, System.StringComparison.InvariantCultureIgnoreCase)).ToList();

            for (int i = displayPage * displayCap; i < translations.Count; i++)
            {
                TranslationCell<AudioClip> temp = translations[i];

                displayedTranslations++;

                EditorGUILayout.BeginHorizontal(listItemStyle);

                temp.key = EditorGUILayout.TextArea(temp.key, editableTextStyle, listItemKeyLayoutOptions);

                for (int j = 0; j < translatedLanguages; j++)
                {
                    temp.translations[j] = (AudioClip)EditorGUILayout.ObjectField(temp.translations[j], typeof(AudioClip), false, listItemLayoutOptions);
                }

                EditorGUILayout.EndHorizontal();

                if (displayedTranslations >= displayCap)
                    return true;
            }

            return false;
        }

        private void DrawTranslationModuleSelection()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            GUILayout.FlexibleSpace();

            EditorGUI.BeginChangeCheck();
            translationModule = (TranslationModule)EditorGUILayout.ObjectField(translationModule, typeof(TranslationModule), false, GUILayout.Width(OBJECT_FIELD_WIDTH), GUILayout.Height(OBJECT_FIELD_HEIGHT));
            if (EditorGUI.EndChangeCheck() && translationModule != null)
            {
                languageCount = translationModule.translatedLanguages.Length;
                scrollPosition = new Vector2();
                displayPage = 0;
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        private Texture2D MakeTex(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}
#endif