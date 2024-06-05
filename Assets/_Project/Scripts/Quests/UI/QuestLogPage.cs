using DualityGame.UI;
using UnityEngine.UIElements;
using NodeCanvas.DialogueTrees;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DualityGame.Quests.UI
{
    public class QuestLogPage: VisualElement
    {
        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;
        private readonly Image _questGiver;

        public new class UxmlFactory : UxmlFactory<QuestLogPage, UxmlTraits> { }

        private Quest.QuestState _questState;

        public Quest.QuestState QuestState
        {
            get => _questState;
            set
            {
                if (_questState == value) return;
                _questState = value;
                UpdateVisuals();
            }
        }

        private void UpdateVisuals()
        {
            if (_questState != null)
            {
                style.display = DisplayStyle.Flex;
                _titleLabel.text = _questState.Quest.TitleWithNPC;
                _descriptionLabel.text = _questState.Quest.Description;
                _questGiver.sprite = _questState.Quest.NPC == null ? null : _questState.Quest.NPC.PortraitByMood(Mood.Neutral);
            }
            else
            {
                style.display = DisplayStyle.None;
            }
        }

        public QuestLogPage()
        {
            style.flexDirection = FlexDirection.Column;
            style.justifyContent = Justify.SpaceBetween;
            
            _titleLabel = new Label
            {
                name = "Title",
                style =
                {
                    flexShrink = 0,
                },
            };
            hierarchy.Add(_titleLabel);

            hierarchy.Add(new Label
            {
                name = "DescriptionHeader",
                text = "Description:",
                style =
                {
                    flexShrink = 0,
                },
            });

            var scrollView = new ScrollView
            {
                style =
                {
                    flexBasis = 0,
                    flexShrink = 0,
                    flexGrow = 1,
                },
                mode = ScrollViewMode.Vertical,
            };
            hierarchy.Add(scrollView);
            
            _descriptionLabel = new Label
            {
                name = "Description",
            };
            _descriptionLabel.style.whiteSpace = WhiteSpace.Normal;
            scrollView.Add(_descriptionLabel);
            
            var bottomRow = new VisualElement
            {
                name = "BottomRow",
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems = Align.Stretch,
                    flexShrink = 0,
                },
            };
            hierarchy.Add(bottomRow);
            
            var questGiverContainer = new AspectRatioContainer
            {
                RatioHeight = 200,
                RatioWidth = 200,
            };
            bottomRow.Add(questGiverContainer);

            _questGiver = new Image
            {
                name = "QuestGiver",
            };
            questGiverContainer.Add(_questGiver);
            
            #if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _titleLabel.text = ExampleTitle;
                _descriptionLabel.text = ExampleDescription;
            }
            else
            {
                UpdateVisuals();
            }
            #else
            UpdateVisuals();
            #endif
        }

        #if UNITY_EDITOR
        private const string ExampleTitle = "The Eyes' have it (Kasadya)";
        private const string ExampleDescription = @"Mortal, you have found one of my many eyes. 
 Return to me all of the eyes you find, there are many but they are all mine, mine to use to gaze into
 souls&quot; Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi varius orci rhoncus ante blandit
 tristique. Suspendisse mollis, mauris nec hendrerit tempor, odio nulla eleifend augue, ut sollicitudin turpis
 ligula at tellus. Donec egestas laoreet urna, finibus laoreet sapien venenatis convallis. Morbi ornare est eu
 neque scelerisque placerat. Nullam porta in lorem vestibulum fermentum. Orci varius natoque penatibus et magnis
 dis parturient montes, nascetur ridiculus mus. Vestibulum pellentesque magna non nunc feugiat commodo. Phasellus
 semper, sem quis dictum sodales, sapien augue bibendum risus, et rhoncus sapien nulla id ipsum. Nam ut dictum
 libero, efficitur hendrerit sapien.&#10;&#10;Phasellus ac pharetra odio, non "; //"scelerisque nisi. Praesent imperdiet tellus purus, eu tincidunt nisl efficitur et. Nunc ullamcorper aliquet risus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer vel elit ac tortor rutrum bibendum ut a odio. Pellentesque sagittis congue lacus, at semper elit maximus eget. Sed tristique nibh at sapien egestas tempus eget in nulla. Sed a arcu sed mauris suscipit malesuada. In hac habitasse platea dictumst.&#10;&#10;Morbi malesuada vitae tellus ut ultricies. Donec dignissim luctus justo, at placerat dui suscipit ac. Nunc tortor eros, iaculis accumsan cursus quis, commodo eget erat. Praesent dolor felis, dapibus a urna eu, finibus fermentum dui. Praesent viverra eu quam eget viverra. Vivamus fringilla dui massa, non luctus nisl interdum eu. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Aenean convallis interdum dui, non facilisis nibh egestas eget. Nunc iaculis diam quis quam ornare, a consectetur odio ultricies. Donec quis urna eu massa molestie malesuada. Nullam condimentum ipsum vel hendrerit hendrerit.&#10;&#10;Aenean blandit metus non orci pulvinar dignissim. Ut semper tellus imperdiet, pulvinar mi in, viverra arcu. Aenean ut dolor eu lacus suscipit bibendum vel sit amet nisi. Nunc quis massa neque. Nulla dapibus convallis purus vitae rhoncus. Etiam bibendum urna vitae felis pulvinar, ac eleifend mi dapibus. Curabitur congue, magna quis vestibulum lobortis, nibh augue pharetra nisl, eu fermentum magna turpis nec purus. Phasellus et ligula at ante pretium fringilla nec sit amet turpis. Fusce orci est, tempus eget eros sit amet, tincidunt vulputate est. Vivamus at iaculis ex. Phasellus ut ex tempus justo finibus cursus et fringilla ex. Aliquam ac tellus eget enim consectetur sodales ut et eros. Morbi tempor commodo leo sed aliquam. Pellentesque augue tellus, rhoncus ut tincidunt id, ullamcorper a odio. Nullam placerat posuere dignissim.&#10;&#10;Nulla facilisi. Quisque pulvinar eros vel velit sodales tincidunt. Pellentesque finibus, mauris vel ullamcorper hendrerit, sem massa pharetra nibh, in efficitur lectus velit dictum leo. Morbi quis laoreet augue, in faucibus lectus. Vivamus in erat finibus, varius urna hendrerit, tempor neque. Phasellus tempus diam vitae lorem porta tincidunt. Aenean odio massa, iaculis a bibendum a, aliquet vel quam.";
        #endif
    }
}
