import * as React from 'react';
import {
    createRulesTreeSelectors,
    RulesTreeSelectors,
} from '~/app/state/playlist/rulesTree/rulesTree.selectors';
import { createPlaylistActions, PlaylistActions } from '~/app/state/playlist/playlist.actions';
import {
    createPlaylistSelectors,
    PlaylistSelectors,
} from '~/app/state/playlist/playlist.selectors';
import { PlaylistAction, PlaylistState } from '~/app/state/playlist/playlist.reducer';
import {
    createRulesTreeActions,
    RulesTreeActions,
} from '~/app/state/playlist/rulesTree/rulesTree.actions';
import { AppContextProps } from '~/app/state/app.context';

export type PlaylistContextProps = Partial<
    RulesTreeActions & RulesTreeSelectors & PlaylistActions & PlaylistSelectors
>;

export const PlaylistContext = React.createContext<PlaylistContextProps>({});

export const createPlaylistContextValue = (
    state: PlaylistState,
    dispatcher: React.Dispatch<PlaylistAction>,
    appContext: AppContextProps,
): PlaylistContextProps => {
    if (!state) {
        throw new Error('state cannot be null');
    }

    const rulesTreeActions = createRulesTreeActions(dispatcher);
    const rulesTreeSelectors = createRulesTreeSelectors(state.rulesTree);
    const rulesCriteriaActions = createPlaylistActions(dispatcher, state, appContext);
    const rulesCriteriaSelectors = createPlaylistSelectors(state);

    return {
        ...rulesTreeActions,
        ...rulesTreeSelectors,
        ...rulesCriteriaActions,
        ...rulesCriteriaSelectors,
    };
};
