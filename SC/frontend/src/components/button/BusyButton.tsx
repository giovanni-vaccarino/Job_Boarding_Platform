import { FC } from 'react';
import CircularProgress from '@mui/material/CircularProgress';
import { Button, ButtonProps } from '@mui/material';

export interface BusyButton extends ButtonProps {
    isBusy: boolean;
}
export const BusyButton: FC<BusyButton> = (props) => {
    const isBusy = props.isBusy;
    const buttonProps: Partial<BusyButton> = { ...props };
    delete buttonProps.isBusy;
    let disabled = true;
    if (props.disabled) {
        disabled = true;
    } else {
        disabled = props.isBusy;
    }
    return (
        <Button
            {...buttonProps}
            disabled={disabled}
            startIcon={isBusy ? <CircularProgress size={15} color="primary" /> : null}
        >
            {props.children}
        </Button>
    );
};