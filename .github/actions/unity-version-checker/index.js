import { setOutput, setFailed } from '@actions/core';
import fetch from 'node-fetch';
import { readFileSync } from 'fs';

async function main() {
    const projFile = readFileSync('./ProjectSettings/ProjectVersion.txt', 'utf8');

    console.log(projFile)

    //console.log(data.match(/m_EditorVersionWithRevision: ([a-z0-9.]*) \(([a-z0-9]*)\)$/))
    const matches = projFile.match(/([a-z0-9.]*) \(([a-z0-9]*)\)/);

    console.log(matches[1]);
    console.log(matches[2]);


    const response = await fetch('https://registry.hub.docker.com/v2/repositories/battlefieldnoob/unity-github-ci/tags?page_size=1024');
    const data = await response.json();

    const OnDockerHub = data.results.map(elem => elem.name);

    console.log(OnDockerHub);

    const isAvailable = OnDockerHub.filter(elem => elem === (matches[1] + "-base")).length === 1;
    setOutput("unity-version", matches[1]);
    setOutput("unity-changeset", matches[2]);
    setOutput("is-image-available", isAvailable);
}

main().catch (error => {
    setFailed(error.message);
})