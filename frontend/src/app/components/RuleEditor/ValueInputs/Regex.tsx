import * as React from 'react';
import { RegexValue } from '~/app/types/rule';
import { Input } from '~/common/components/Input';
import { Label } from '~/common/components/Label';
import { MenuInput } from '~/common/components/MenuInput';
import { Toggle } from '~/common/components/Toggle';
import { showError } from '~/common/helpers/utils';

type RegexValueInputProps = {
    value: RegexValue;
    onChange(value: RegexValue): void;
};

export const RegexValueInput: React.FC<RegexValueInputProps> = props => {
    function checkCompiledRegex(str) {
        try {
            new RegExp(str);
        } catch (error) {
            return false;
        }
        return true;
    }

    return (
        <MenuInput
            value={props.value.value}
            onBlur={e => {
                if (checkCompiledRegex(e.target.value))
                    props.onChange({
                        ...props.value,
                        value: e.target.value,
                    })
                else {
                    showError({ label: 'Error', content: 'Invalid regular expression', modal: true, timeout: 3000 })
                    e.currentTarget.value = ''
                }
            }}
            menuItems={[{
                label: 'Case Sensitive',
                onToggle: (check) => props.onChange({
                    ...props.value,
                    caseSensitive: check
                }),
                value: props.value.caseSensitive
            }]}
        />
    );
};
