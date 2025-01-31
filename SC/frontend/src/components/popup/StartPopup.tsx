import { useState } from 'react';
import { styled } from '@mui/system';
import { useNavigate } from 'react-router-dom';
import { AppRoutes } from '../../router.tsx';

export function StartPopup() {
  const [open, setOpen] = useState(true); // Initially, the popup is open.
  const navigate = useNavigate();

  const handleStart = () => {
    setOpen(false); // Close the popup.
  };

  const handleCancel = () => {
    setOpen(false); // Close the popup.
    navigate(AppRoutes.Home);
  };

  return (
    <>
      {open && (
        <Overlay>
          <PopupContainer>
            <PopupMessage>Do you want to start?</PopupMessage>
            <ButtonContainer>
              <PopupButton onClick={handleStart}>Start the test</PopupButton>
              <PopupButton onClick={handleCancel} variant="cancel">
                No, Later
              </PopupButton>
            </ButtonContainer>
          </PopupContainer>
        </Overlay>
      )}
    </>
  );
}

const Overlay = styled('div')`
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.5); /* Semi-transparent background */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999; /* Ensure it overlays everything */
`;

const PopupContainer = styled('div')`
  background-color: #fff;
  padding: 24px 32px;
  border-radius: 12px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  text-align: center;
  max-width: 250px;
  width: 40%;
`;

const PopupMessage = styled('h2')`
  font-family: 'Arial', sans-serif;
  font-size: 1.3rem;
  margin-bottom: 20px;
`;

const ButtonContainer = styled('div')`
  display: flex;
  gap: 14px;
  justify-content: center;
`;

const PopupButton = styled('button')<{ variant?: string }>`
  padding: 10px 20px;
  font-size: 1rem;
  border-radius: 20px;
  cursor: pointer;
  font-weight: bold;
  background-color: ${(props) =>
    props.variant === 'cancel' ? '#f44336' : '#233965'};
  color: #fff;
  border: none;
  transition: background-color 0.3s ease;

  &:hover {
    background-color: ${(props) =>
      props.variant === 'cancel' ? '#9e4b4b' : '#477393'};
  }

  &:active {
    background-color: ${(props) =>
      props.variant === 'cancel' ? '#c62828' : '#2e7d32'};
  }
`;
