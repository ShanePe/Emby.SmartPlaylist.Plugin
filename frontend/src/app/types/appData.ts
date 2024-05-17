import { PlaylistBasicData, PlaylistRulesTree } from '~/app/types/playlist';
import { RuleCriteriaDefinition, RuleOrRuleGroup } from '~/app/types/rule';
import { TreeNodeData } from '~/common/components/TreeView/types/tree';

export type AppPlaylist = PlaylistBasicData &
    PlaylistRulesTree<Array<TreeNodeData<RuleOrRuleGroup>>>;

export type Source = {
    type: string,
    id: string,
    name: string
}

export type User = {
    id: string,
    name: string
}

export type AppData = {
    appId: string;
    rulesCriteriaDefinitions: RuleCriteriaDefinition[];
    limitOrdersBy: string[];
    sources: Source[],
    users: User[]
} & AppPlaylists;

export type AppPlaylists = {
    playlists: AppPlaylist[];
};

export type Upload = {
    uploadFile: File,
    type: string,
    onProgress?(ev: ProgressEvent<EventTarget>): void
    onStart?(): void,
    onEnd?(): void
}

export type HierarchyString = {
    value: string;
    children: HierarchyString[];
    level: number;
}