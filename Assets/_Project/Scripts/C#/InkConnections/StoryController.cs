using Ink.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VVGames.Common;
using VVGames.ForgottenTrails.SaveLoading;
using static VVGames.ForgottenTrails.InkConnections.StoryController.InterfaceBroking;
using static VVGames.ForgottenTrails.InkConnections.StoryController.TextProduction;

namespace VVGames.ForgottenTrails.InkConnections
{
    /// <summary>
    /// <para>Behaviour for parsing content from <see cref="Story"/> files and passing it onto the appropriate monobehaviours.</para>
    /// </summary>
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Fields

        internal SCDummyState dummyState = new(); //feels like I could have a more clean way of declaring a state machine
        internal SCSuperState superState = new();
        internal SCProductionState productionState = new();
        internal SCFunctionState functionState = new();
        internal SCWaitingForInputState waitingForInputState = new();
        internal SCWaitingForChoiceState waitingForChoiceState = new();
        internal SCWaitingForContinueState waitingForContinueState = new();
        internal SCBookMenuState bookMenuState = new();
        internal SCSettingsState settingsState = new();
        internal SCDataState dataState = new();
        internal SCInventoryState inventoryState = new();
        internal SCMapState mapState = new();
        internal SCPartyState partyState = new();
        internal SCSavingState savingState = new();

        private const string dataLabel = "BasicInkScript"; // DEPRECATED: labels are remaining artifact from when I thought there would be many different data objects

        [SerializeField, BoxGroup("Data"), ReadOnly]
        [Tooltip("View data object containing INK data.")]
        private StoryData _inkDataAsset = null;

        #endregion Fields

        #region Properties

        [field: SerializeField, ReadOnly, BoxGroup("State Info")]
        public StackBasedStateMachine<StoryController> StateMachine { get; private set; }

        public TextAsset InkStoryAsset
        { get { return AssetManager.Instance.TextAsset; } set { AssetManager.Instance.TextAsset = value; } }

        public StoryData InkDataAsset
        {
            get
            {
                if (_inkDataAsset == null)
                {
                    _inkDataAsset = CreateBlankData();
                }
                return _inkDataAsset;
            }
            internal set
            {
                _inkDataAsset = value;
            }
        }

        public Story Story { get; private set; }

        [field: SerializeField, BoxGroup("TextProducer")]
        internal TextProduction TextProducer { get; set; }

        [field: SerializeField, BoxGroup("SetDresser")]
        internal SetDressing SetDresser { get; set; }

        [field: SerializeField, BoxGroup("InterfaceBroker")]
        internal InterfaceBroking InterfaceBroker { get; set; }

        [field: SerializeField, BoxGroup("SceneReferences")]
        internal TMPro.TMP_InputField InputField { get; set; }

        #endregion Properties

        #region Public Methods

        [Tooltip("Reset ink data in object. Note: does not remove data from file")]
        [Button("ResetInkData", EButtonEnableMode.Editor)] public void ResetInkDataButton() => ResetData();

        [Tooltip("Load data from disk and reset scene.")]
        [Button("ResetScene", EButtonEnableMode.Playmode)] public void ResetSceneButton() => ResetScene();

        public void AssignName()
        {
            Story.state.variablesState["PlayerName"] = DataManager.Instance.MetaData.playerName = InputField.text;
            InputField.DeactivateInputField();
            InputField.gameObject.SetActive(false);
            waitingForInputState.DropCondition = true;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();

            transform.localPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y); // I don't remember why this is here (as of 2024-01-11)

            //book = FindFirstObjectByType<Book>();
            SetDresser.Assign();
            TextProducer.Assign();
            InterfaceBroker.Assign();
            InitialiseStateMachine();
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitialiseStateMachine()
        {
            //Debug.Log("Making state machine.");
            StateMachine = new
            (
            this,
            dummyState,
            superState,
            productionState,
            functionState,
            waitingForInputState,
            waitingForChoiceState,
            waitingForContinueState,
            savingState,
            bookMenuState,
            settingsState,
            dataState,
            inventoryState,
            partyState,
            mapState
            );
        }

        private void Update()
        {
            if (StateMachine.CurrentState != null)
            {
                StateMachine.Update();
            }
        }

        private void OnDestroy()
        {
            InkDataAsset = null;
        }

        // Public Methods

        // Private Methods

        /// <summary>
        /// Make a new <see cref="StoryData"/>object
        /// </summary>
        /// <returns>The freshly made blank data</returns>
        private StoryData CreateBlankData(bool forBootup = false)
        {
            // NOTE: this feels like a bit of a hacky way to create new data
            StoryData data = new();
            if (!forBootup)
            {
                Debug.Log("Created new data " + data.Label);
            }
            return data;
        }

        private void ResetData()
        {
            InkDataAsset = CreateBlankData();
        }

        private void ResetScene()
        {
            if (UnityEditor.EditorApplication.isPlaying == true)
            {
                StopScene(); // should this be done from the statemachine???
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //StateMachine.Reset();
                //StateMachine.TransitionToState(StoryController.Instance.superState);
            }
        }

        private void StopScene()
        {
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                StopAllCoroutines();
            }
        }

        private void PromptName()
        {
            StateMachine.TransitionToState(waitingForInputState);
            InputField.gameObject.SetActive(true);
            InputField.ActivateInputField();
        }

        private void ConsoleLogInk(string text, bool warning = false)
        {
            if (warning)
            {
                Debug.LogWarning("Warning from INK Script: " + text);
            }
            else
            {
                Debug.Log("Message from INK Script: " + text);
            }
        }

        #endregion Private Methods
    }
}