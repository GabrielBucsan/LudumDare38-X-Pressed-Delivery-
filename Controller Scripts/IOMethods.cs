using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

//Class that manages saving and loading data to/from file
public class IOMethods : MonoBehaviour {

	//Method called to save data from game to file
	public void Save(){

		//Opening file
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream stream = new FileStream (Application.persistentDataPath + "/" + "save.gbu", FileMode.Create);

		//Arranging data to save
		DataToSave data = new DataToSave ();
		SaveDataFromGame (data);

		bf.Serialize (stream, data);
		stream.Close ();

	}

	//Method called to load data from file to game
	public void Load(){


		if (System.IO.File.Exists (Application.persistentDataPath + "/" + "save.gbu")) {

			//Opening file
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream stream = new FileStream (Application.persistentDataPath + "/" + "save.gbu", FileMode.Open);

			//Getting data to load
			DataToSave data = new DataToSave ();
			data = (DataToSave)bf.Deserialize (stream);
			stream.Close ();

			LoadDataFromFile (data);

		} 
	}

	//Method that assigns the serializableclass variables from game to file
	public void SaveDataFromGame(DataToSave _data){

		_data.highScore = GameController.instance.highscore;

	}

	//Method that assigns the serializableclass variables from file to game
	public void LoadDataFromFile(DataToSave _data){

		GameController.instance.highscore = _data.highScore;

	}
}

//Serializable class to store data
[Serializable]
public class DataToSave{

	public int highScore;

	public static byte [] ToBytes (DataToSave data) {

		var formatter = new BinaryFormatter ();

		using (var stream = new MemoryStream ()) {

			formatter.Serialize (stream, data);
			return stream.ToArray ();
		}
	}

	// Convert byte array to class instance
	public static DataToSave FromBytes (byte [] data) {

		using (var stream = new MemoryStream ()) {

			var formatter = new BinaryFormatter ();
			stream.Write (data, 0, data.Length);
			stream.Seek (0, SeekOrigin.Begin);

			DataToSave block = (DataToSave) formatter.Deserialize (stream);
			return block;
		}
	}

}
