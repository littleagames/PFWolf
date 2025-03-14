﻿using System.Reflection;
using LittleAGames.PFWolf.Common;
using LittleAGames.PFWolf.Common.FileLoaders;
using LittleAGames.PFWolf.Common.Helpers;

namespace Engine.Managers;

public class AssetManager : IAssetManager
{
    private readonly FileLoader _fileLoader;
    private readonly List<Asset> _assets = new List<Asset>();

    public AssetManager(FileLoader fileLoader)
    {
        _fileLoader = fileLoader;
    }
    
    public int AssetCount => _assets.Count;
    
    // TODO: This will take in a string "asset" and return the metadata, and byte data for that asset
    public void AddGamePack(GamePack gamePack, string directory)
    {
        var gamePackAssets = gamePack.LoadAssets(directory);
        _assets.AddRange(gamePackAssets);
    }

    public void AddModPack(string directory, string pk3File)
    {
        var loader = new Pk3FileLoader(directory, pk3File);
        var pk3Assets = loader.Load();
        _assets.AddRange(pk3Assets);
    }

    public void AddAssembly(Assembly assembly)
    {
        var scriptAssets = AssemblyScriptHelper.LoadScriptsFromAssembly(assembly);
        foreach (var scriptAsset in scriptAssets)
        {
            var existing = _assets.FirstOrDefault(x =>
                scriptAsset.Name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
            if (existing != null)
                _assets.Remove(existing);
            
            _assets.Add(scriptAsset);
        }
    }
    
    public List<Asset> GetAssets(AssetType assetType)
        => _assets.Where(a => a.AssetType == assetType).ToList();
    
    public List<T> GetAssets<T>(AssetType assetType) where T : Asset
        => _assets.Where(a => a.AssetType == assetType).Select(x => (T)x).ToList();

    public Asset? FindAsset(AssetType assetType, string assetName)
    {
        return FindAsset<Asset>(assetType, assetName);
    }
    
    public T? FindAsset<T>(AssetType assetType, string assetName) where T : Asset
    {
        return (T?)_assets.FirstOrDefault(a =>
            a.AssetType == assetType && a.Name.Equals(assetName, StringComparison.InvariantCultureIgnoreCase));
    }
    
    public IEnumerable<T> FindAssets<T>(AssetType assetType, IEnumerable<string> assetNames) where T : Asset
    {
        var found = _assets.Where(a =>
            a.AssetType == assetType && assetNames.Contains(a.Name, StringComparer.InvariantCultureIgnoreCase));
        return found.Select(x => x as T);
    }
}