import * as React from 'react';
import { PlaylistContext } from '~/app/state/playlist/playlist.context';
import { Rule } from '~/app/types/rule';
import { Select } from '~/common/components/Select';
import { AppContext } from '~/app/state/app.context';
import { ValueInput } from '~/app/components/RuleEditor/ValueInput';
import { UserListInput } from './ValueInputs/UserList';
import './RuleEditor.css'
type RuleProps = {
    value: Rule;
};

export const RuleEditor: React.FC<RuleProps> = props => {
    const rule = props.value;
    const { criteria } = rule;
    const playlistContext = React.useContext(PlaylistContext);
    const appContext = React.useContext(AppContext);

    const criteriaDefs = appContext.getRulesCriteriaDefinitions();
    const criteriaNames = criteriaDefs.map(x => x.name);
    const operators = appContext.getRuleCriteriaOperators(criteria.name);
    const criteriaDef = appContext.getRulesCriteriaDefinition(criteria.name);

    return (
        <>
            <Select
                values={criteriaNames}
                value={criteria.name}
                onChange={newVal => playlistContext.changeCriteriaDef(rule, newVal)}
            />
            <Select
                values={operators.map(x => x.name)}
                value={criteria.operator.name}
                onChange={newVal => playlistContext.changeOperator(rule, newVal)}
            />

            <ValueInput
                type={criteria.operator.type}
                value={criteria.value}
                values={criteriaDef.values}
                onChange={newVal => {
                    playlistContext.updateCriteriaValue(rule, newVal)
                }}
            />

            {
                criteriaDef.isUserSpecific && (
                    <>
                        <div className='selectContainer padding-lr icon-align'>
                            <div className="md-icon" title='Do the match in the context of the user.'>
                                account_circle
                            </div>
                        </div>
                        <UserListInput
                            userId={criteria.userId}
                            onChange={userId => {
                                playlistContext.updateCriteriaUser(rule, userId)
                            }}
                        />
                    </>
                )
            }
        </>
    );
};
